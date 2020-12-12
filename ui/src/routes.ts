import { RouteConfig } from "vue-router";
import StartUp from "@/components/StartUp.vue";
import Home from "@/components/Home.vue";

const STARTUP_PATH = "/";
const HOME_PATH_PREFIX = "/home/";
const HOME_PATH = HOME_PATH_PREFIX + ":device";

const routes: Array<RouteConfig> = [
    {
        component: StartUp,
        path: STARTUP_PATH
    },
    {
        component: Home,
        path: HOME_PATH,
        props: true
    }
];

export { STARTUP_PATH, HOME_PATH_PREFIX, routes };
