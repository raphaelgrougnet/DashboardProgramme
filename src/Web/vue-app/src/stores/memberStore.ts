import {defineStore} from 'pinia'
import {IAuthenticatedMember} from "@/types/entities/authenticatedMember";
import {Role} from "@/types";

interface MemberState {
    member: IAuthenticatedMember
}

export const useMemberStore = defineStore('member', {
    state: (): MemberState => ({
        member: {roles: [] as string[]} as IAuthenticatedMember
    }),

    actions: { // methods
        setMember(member: IAuthenticatedMember) {
            this.member = member
        },
        hasRole(role: Role): boolean {
            return this.member.roles.some(x => x === role)
        },
        hasOneOfTheseRoles(roles: Role[]): boolean {
            return roles.some(x => this.member.roles.includes(x));
        }
    },

    getters: { // computed
        getMember: state => state.member,
    },

    persist: {
        storage: sessionStorage
    }
});