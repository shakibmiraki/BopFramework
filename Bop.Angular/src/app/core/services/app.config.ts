import { InjectionToken } from "@angular/core";

export let APP_CONFIG = new InjectionToken<string>("app.config");

const apiEndpoint = "http://localhost:5000";

export interface IAppConfig {
  config: {
    tokenKey: string;
    accessTokenObjectKey: string;
    refreshTokenObjectKey: string;
    adminRoleName: string;
  };
  paths: {
    loginPath: string;
    logoutPath: string;
    registerPath: string;
    activatePath: string;
    resendPath: string;
    verifyAccessPath: string;
    refreshTokenPath: string;
    exportLanguagePath: string;
    importLanguagePath: string;
    getAllLanguagePath: string;
  };
}

export const AppConfig: IAppConfig = {
  config: {
    tokenKey: "user_id",
    accessTokenObjectKey: "accessToken",
    refreshTokenObjectKey: "refreshToken",
    adminRoleName: "Administrator",
  },
  paths: {
    loginPath: `${apiEndpoint}/api/auth/login`,
    logoutPath: `${apiEndpoint}/api/auth/logout`,
    registerPath: `${apiEndpoint}/api/auth/register`,
    activatePath: `${apiEndpoint}/api/auth/activate`,
    resendPath: `${apiEndpoint}/api/auth/resend`,
    verifyAccessPath: `${apiEndpoint}/admin/user/authorize`,
    refreshTokenPath: `${apiEndpoint}/admin/user/refreshtoken`,
    exportLanguagePath: `${apiEndpoint}/admin/localization/exportlanguage`,
    importLanguagePath: `${apiEndpoint}/admin/localization/importlanguage`,
    getAllLanguagePath: `${apiEndpoint}/admin/localization/getalllanguages`,
  },
};
