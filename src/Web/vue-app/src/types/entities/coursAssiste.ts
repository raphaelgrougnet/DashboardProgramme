import {Cours} from "@/types/entities/cours";
import {SessionAssistee} from "@/types/entities/sessionAssistee";
import {Note} from "@/types/enums/note";

export class CoursAssiste {
    cours?: Cours;
    noteRecue?: Note;
    numeroGroupe?: number;
    sessionAssistee?: SessionAssistee;
}