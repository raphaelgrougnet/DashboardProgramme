import i18n from "@/i18n";
import {Role} from "@/types";
import {useMemberStore} from "@/stores/memberStore";
import {createRouter, createWebHistory} from "vue-router";

import Admin from "../views/admin/Admin.vue";
import AdminMembersIndex from "@/views/admin/AdminMembersIndex.vue";
import AdminAddProgrammeForm from "@/views/admin/AdminAddProgrammeForm.vue";
import AdminProgrammeIndex from "@/views/admin/AdminProgrammeIndex.vue";
import AdminEditProgrammeForm from "@/views/admin/AdminEditProgrammeForm.vue";
import AdminAddMemberForm from "@/views/admin/AdminAddMemberForm.vue";
import Index from "@/views/programme/Index.vue";
import SessionsProgramme from "@/views/programme/SessionsProgramme.vue";
import SessionProgramme from "@/views/session/SessionProgramme.vue";

import CoursConsulter from "@/views/cours/CoursConsulter.vue";
import AdminEditMemberForm from "@/views/admin/AdminEditMemberForm.vue";
import ImportData from "@/views/gestionnaire/ImportData.vue";


const router = createRouter({
    // eslint-disable-next-line
    scrollBehavior(to, from, savedPosition) {
        // always scroll to top
        return {top: 0};
    },
    history: createWebHistory(),
    routes: [
        {
            path: i18n.t("routes.programme.path"),
            alias: i18n.t("routes.home.path"),
            name: "programme",
            component: Index,
        },
        {
            path: i18n.t("routes.programme.children.:id.fullPath"),
            name: "programme:id",
            component: SessionsProgramme,
        },
        {
            path: i18n.t("routes.programme.children.:id.children.session.fullPath"),
            redirect: {name: "programme:id"},
        },
        {
            path: i18n.t("routes.programme.children.:id.children.session.children.:id.fullPath"),
            name: "programme:id.session:id",
            component: SessionProgramme,
        },
        {
            path: i18n.t("routes.programme.children.:id.children.session.children.:id.children.cours.children.:id.fullPath"),
            name: "programme:id.session:id.cours:id",
            component: CoursConsulter,
        },
        {
            path: i18n.t("routes.import.path"),
            name: 'import',
            component: ImportData,
            meta: {
                requiredRole: Role.Admin,
                noLinkInBreadcrumbs: true,
            }
        },

        {
            path: i18n.t("routes.admin.path"),
            name: "admin",
            component: Admin,
            meta: {
                requiredRole: Role.Admin,
                noLinkInBreadcrumbs: true,
            },
            children: [
                {
                    path: i18n.t("routes.admin.children.members.path"),
                    name: "admin.children.members",
                    component: Admin,
                    children: [
                        {
                            path: "",
                            name: "admin.children.members.index",
                            component: AdminMembersIndex,
                        },
                        {
                            path: i18n.t("routes.admin.children.members.add.path"),
                            name: "admin.children.members.add",
                            component: AdminAddMemberForm,
                        },
                        {
                            path: i18n.t("routes.admin.children.members.edit.path"),
                            name: "admin.children.members.edit",
                            component: AdminEditMemberForm,
                            props: true
                        }
                    ],
                },
                {
                    path: i18n.t("routes.admin.children.programmes.path"),
                    name: "admin.children.programmes",
                    component: Admin,
                    children: [
                        {
                            path: "",
                            name: "admin.children.programmes.index",
                            component: AdminProgrammeIndex,
                        },
                        {
                            path: i18n.t("routes.admin.children.programmes.add.path"),
                            name: "admin.children.programmes.add",
                            component: AdminAddProgrammeForm,
                        },
                        {
                            path: i18n.t("routes.admin.children.programmes.edit.path"),
                            name: "admin.children.programmes.edit",
                            component: AdminEditProgrammeForm,
                            props: true
                        }
                    ],
                }
            ]
        },
    ]
});

// eslint-disable-next-line
router.beforeEach(async (to, from) => {
    const memberStore = useMemberStore();
    if (!to.meta.requiredRole)
        return;
    const isRoleArray = Array.isArray(to.meta.requiredRole);
    const doesNotHaveGivenRole = !isRoleArray && !memberStore.hasRole(to.meta.requiredRole as Role);
    const hasNoRoleAmountRoleList = isRoleArray && !memberStore.hasOneOfTheseRoles(to.meta.requiredRole as Role[]);
    if (doesNotHaveGivenRole || hasNoRoleAmountRoleList) {
        return {
            name: "programme",
        };
    }
});

export const Router = router;