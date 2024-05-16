import {Container} from "inversify";
import axios, {AxiosInstance} from 'axios';
import "reflect-metadata";

import {TYPES} from "@/injection/types";
import {
    IApiService,
    ICorrelationsServices,
    ICoursAssisteService,
    ICoursService,
    IEtudiantService,
    IImportDataService,
    IMemberService,
    IPortraitEtudiantService,
    IProgrammeService,
    ISessionEtudeService,
    ISpeQueryService
} from "@/injection/interfaces";
import {ApiService, CoursService, EtudiantService, MemberService, ProgrammeService} from "@/services";
import {SessionEtudeService} from "@/services/sessionEtudeService";
import {CoursAssisteService} from "@/services/coursAssisteService";
import {CorrelationsServices} from "@/services/correlationsServices";
import {SpeQueryService} from "@/services/speQueryService";
import {ImportDataService} from "@/services/importDataService";
import {PortraitEtudiantServices} from "@/services/portraitEtudiantServices";

const dependencyInjection = new Container();
dependencyInjection.bind<AxiosInstance>(TYPES.AxiosInstance).toConstantValue(axios.create());
dependencyInjection.bind<IApiService>(TYPES.IApiService).to(ApiService).inSingletonScope();
dependencyInjection.bind<IMemberService>(TYPES.IMemberService).to(MemberService).inSingletonScope();
dependencyInjection.bind<IProgrammeService>(TYPES.IProgrammeService).to(ProgrammeService).inSingletonScope();
dependencyInjection.bind<IEtudiantService>(TYPES.IEtudiantService).to(EtudiantService).inSingletonScope();
dependencyInjection.bind<ISessionEtudeService>(TYPES.ISessionEtudeService).to(SessionEtudeService).inSingletonScope();
dependencyInjection.bind<ICoursAssisteService>(TYPES.ICoursAssisteService).to(CoursAssisteService).inSingletonScope();
dependencyInjection.bind<ICoursService>(TYPES.ICoursService).to(CoursService).inSingletonScope();
dependencyInjection.bind<ICorrelationsServices>(TYPES.ICorrelationsServices).to(CorrelationsServices).inSingletonScope();
dependencyInjection.bind<IPortraitEtudiantService>(TYPES.IPortraitEtudiantService).to(PortraitEtudiantServices).inSingletonScope();
dependencyInjection.bind<ISpeQueryService>(TYPES.ISPEQueryService).to(SpeQueryService).inSingletonScope();

dependencyInjection.bind<IImportDataService>(TYPES.IImportDataService).to(ImportDataService).inSingletonScope();

function useMemberService() {
    return dependencyInjection.get<IMemberService>(TYPES.IMemberService);
}

function useProgrammeService() {
    return dependencyInjection.get<IProgrammeService>(TYPES.IProgrammeService);
}

function useEtudiantService() {
    return dependencyInjection.get<IEtudiantService>(TYPES.IEtudiantService);
}

function useSessionEtudeService() {
    return dependencyInjection.get<ISessionEtudeService>(TYPES.ISessionEtudeService);
}

function useCoursAssisteService() {
    return dependencyInjection.get<ICoursAssisteService>(TYPES.ICoursAssisteService);
}

function useCoursService() {
    return dependencyInjection.get<ICoursService>(TYPES.ICoursService);
}

function useCorrelationsService() {
    return dependencyInjection.get<ICorrelationsServices>(TYPES.ICorrelationsServices);
}

function useSpeQueryService() {
    return dependencyInjection.get<ISpeQueryService>(TYPES.ISPEQueryService);
}


function useIportDataService() {
    return dependencyInjection.get<IImportDataService>(TYPES.IImportDataService);
}

function usePortraitEtudiantService() {
    return dependencyInjection.get<IPortraitEtudiantService>(TYPES.IPortraitEtudiantService);
}

export {
    dependencyInjection,
    useMemberService,
    useProgrammeService,
    useEtudiantService,
    useSessionEtudeService,
    useCoursAssisteService,
    useCoursService,
    useCorrelationsService,
    useSpeQueryService,
    usePortraitEtudiantService,
    useIportDataService,
};