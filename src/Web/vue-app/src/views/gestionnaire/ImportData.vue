<template>

  <div class="d-flex flex-column">
    <div style="height: 200px;">
      <h1 class="mx-auto my-5 fs-1 fw-bold">{{ t("global.importFiles") }}</h1>
    </div>
    <form id="file-upload-form" class="uploader" @submit="handleFormSubmit">
      <input id="file-upload" ref="inputFileUpload" accept=".xls,.xlsx" type="file" @change="onSelectFichier"/>

      <label for="file-upload">


        <img id="file-image" alt="Preview" class="hidden" src="#">
        <div id="start">
          <div v-if="file">
            <i aria-hidden="true" class="fa fa-file-excel-o"></i>
            <p v-if="file">{{ file.name }}</p>

          </div>
          <div v-else>
            <i aria-hidden="true" class="fa fa-download"></i>

          </div>
          <div class="d-flex justify-content-around">
            <span id="file-upload-btn" class="btn btn-primary">{{ t("global.selectFile") }}</span>
            <button class="btn btn--icon-translate-hover " type="submit">
              {{ t('routes.import.name') }}
              <IconArrow class="icon"/>

            </button>
          </div>

        </div>


      </label>

      <Loader v-if="enchargement"/>


    </form>

  </div>


</template>

<script lang="ts" setup>
import * as XLSX from 'xlsx';
import {ref, Ref} from "vue";
import {useI18n} from "vue3-i18n";
import {useIportDataService} from "@/inversify.config";
import Loader from "@/components/layout/Loader.vue";
import SchemaJson from "@/validation/schema-importation.json";
import Ajv from "ajv";

import IconArrow from "@/assets/icons/icon__arrow.svg";
import {notify} from "@kyvg/vue3-notification";
import {notifyError} from "@/notify";
import {useRouter} from "vue-router";
// import {RecordRequest} from "@/types";

const inputFileUpload: Ref<HTMLInputElement | null> = ref(null);
const importDataService = useIportDataService();
const enchargement = ref(false);

const router = useRouter();

const file = ref();
const {t} = useI18n();
//const Ajv = require("ajv")
const ajv = new Ajv();

const onSelectFichier = () => {
  if (inputFileUpload.value === null || !inputFileUpload.value.files?.length) {
    notifyError("Aucun fichier sélectionné");
    throw new Error("Aucun fichier sélectionné");
  }
  file.value = inputFileUpload.value!.files?.[0];
};

const obtenirContenuFichier = (): Promise<[string, Record<string, any>[]]> => new Promise((resolve, reject) => {
  if (inputFileUpload.value === null || !inputFileUpload.value.files?.length) {
    notifyError("Aucun fichier sélectionné");
    return reject(new Error("Aucun fichier sélectionné"));
  }

  if (!file.value) {
    return reject(new Error("Aucun fichier sélectionné"));
  }

  const reader = new FileReader();

  reader.onload = (e: ProgressEvent<FileReader>) => {
    if (!e.target) {
      // Ceci me permet de gérer le cas où il n'y a pas de cible dans l'événement
      return reject(new Error("Aucun fichier sélectionné"));
    }

    const data = new Uint8Array(e.target.result as ArrayBuffer);
    const workbook = XLSX.read(data, {type: 'array'});
    const sheetName = workbook.SheetNames[0];
    const worksheet = workbook.Sheets[sheetName];
    const arr = XLSX.utils.sheet_to_json(worksheet, {header: 1});
    const colonnes = arr[0] as string[];

    return resolve(
        [
          sheetName,
          arr.slice(1).map((ligne: any) =>
              colonnes.reduce((acc, colonne: string, index) => {
                acc[colonne] = ligne[index];
                return acc;
              }, {} as any))]
    );
  };

  reader.readAsArrayBuffer(file.value);
});

const handleFormSubmit = async (e: SubmitEvent) => {
  e.preventDefault();

  if (inputFileUpload.value === null || !inputFileUpload.value.files?.length || !file.value) {
    notifyError("Aucun fichier sélectionné");
    return;
  }


  enchargement.value = true;


  const validate = ajv.compile(SchemaJson);

  const [sheetName, records] = await obtenirContenuFichier();


  const req = {
    records: records.filter(record => record["Code permanent crypté"].trim() !== ""),
    sheetName,
  };

  req.records.forEach((record: any) => {
    if (typeof record["GENMELS"] === 'string') {
      var cleanValue = record["GENMELS"].replace(',', '.').trim();
      var numberValue = Number(cleanValue);
      record["GENMELS"] = numberValue;
    }
  });


  console.log("req", req);
  const valid = validate(req);

  if (!valid && validate.errors) {
    notify({
      type: "error",
      title: "Erreur",
      text: validate.errors[0].message,
    });
  } else {

    var response = await importDataService.importData(JSON.stringify(req));
    enchargement.value = false;

    if ('json' in response.errors) {
      (response.errors.json as any[]).forEach((err: any) =>
          notify({
            type: "error",
            title: "Erreur",
            text: err.toString(),
          }));

    } else if (response.errors.length > 0) {
      response.errors.forEach((err: any) =>
          notify({
            type: "error",
            title: "Erreur",
            text: err.errorMessage,
          }));
    } else {
      notify({
        type: "success",
        title: "Succès",
        text: "Importation réussie",
      });
      await router.push({name: "programme"});
    }


  }
  enchargement.value = false;
}
</script>


<style lang="scss" scoped>
@import url(https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css);

$theme: #454cad;
$dark-text: #5f6982;

.uploader {
  display: block;
  clear: both;
  margin: 0 auto;
  width: 100%;
  max-width: 600px;

  label {
    float: left;
    clear: both;
    width: 100%;
    padding: 2rem 1.5rem;
    text-align: center;
    background: #fff;
    border-radius: 7px;
    border: 3px solid #eee;
    transition: all .2s ease;
    user-select: none;

    &:hover {
      border-color: $theme;
    }

    &.hover {
      border: 3px solid $theme;
      box-shadow: inset 0 0 0 6px #eee;

      #start {
        i.fa {
          transform: scale(0.8);
          opacity: 0.3;
        }
      }
    }
  }

  #start {
    float: left;
    clear: both;
    width: 100%;

    &.hidden {
      display: none;
    }

    i.fa {
      font-size: 50px;
      margin-bottom: 1rem;
      transition: all .2s ease-in-out;
    }
  }

  #response {
    float: left;
    clear: both;
    width: 100%;

    &.hidden {
      display: none;
    }

    #messages {
      margin-bottom: .5rem;
    }
  }

  #file-image {
    display: inline;
    margin: 0 auto .5rem auto;
    width: auto;
    height: auto;
    max-width: 180px;

    &.hidden {
      display: none;
    }
  }

  #notimage {
    display: block;
    float: left;
    clear: both;
    width: 100%;

    &.hidden {
      display: none;
    }
  }

  progress,
  .progress {
    // appearance: none;
    display: inline;
    clear: both;
    margin: 0 auto;
    width: 100%;
    max-width: 180px;
    height: 8px;
    border: 0;
    border-radius: 4px;
    background-color: #eee;
    overflow: hidden;
  }

  .progress[value]::-webkit-progress-bar {
    border-radius: 4px;
    background-color: #eee;
  }

  .progress[value]::-webkit-progress-value {
    background: linear-gradient(to right, darken($theme, 8%) 0%, $theme 50%);
    border-radius: 4px;
  }

  .progress[value]::-moz-progress-bar {
    background: linear-gradient(to right, darken($theme, 8%) 0%, $theme 50%);
    border-radius: 4px;
  }

  input[type="file"] {
    display: none;
  }

  div {
    margin: 0 0 .5rem 0;
    color: $dark-text;
  }

  .btn {
    display: inline-block;
    margin: .5rem .5rem 1rem .5rem;
    clear: both;
    font-family: inherit;
    font-weight: 700;
    font-size: 14px;
    text-decoration: none;
    text-transform: initial;
    border: none;
    border-radius: .2rem;
    outline: none;
    padding: 0 1rem;
    height: 36px;
    line-height: 36px;
    color: #fff;
    transition: all 0.2s ease-in-out;
    box-sizing: border-box;
    background: $theme;
    border-color: $theme;
    cursor: pointer;
  }
}
</style>