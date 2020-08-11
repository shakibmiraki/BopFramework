import { api } from "./api";
import { config } from "./../config";
import { utilService } from "./utils";
import { userService } from "./user";
import { tokenService } from "./token";

const register = async (model) => {
  const value = {
    mobile: utilService.normalizeMobile(model.mobile),
    password: model.password,
    confirmPassword: model.confirmPassword,
  };
  return api
    .post(`${config.apiUrl}/api/auth/register`, value)
    .then((result) => {
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const resend = async (model) => {
  const value = {
    mobile: utilService.normalizeMobile(userService.getMobile()),
    code: model.code,
  };
  return api
    .post(`${config.apiUrl}/api/auth/resend`, value)
    .then((result) => {
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const activate = async (model) => {
  const value = {
    mobile: utilService.normalizeMobile(userService.getMobile()),
    code: model.code,
  };

  return api
    .post(`${config.apiUrl}/api/auth/activate`, value)
    .then((result) => {
      console.log(result);
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const login = async (model) => {
  const value = {
    mobile: utilService.normalizeMobile(model.mobile),
    password: model.password,
    rememberMe: model.rememberMe,
  };

  return api
    .post(`${config.apiUrl}/api/auth/login`, value)
    .then((result) => {
      console.log(result);
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const logout = async () => {
  const refreshToken = encodeURIComponent(tokenService.getRefreshToken());
  return api
    .get(`${config.apiUrl}/api/auth/logout?refreshToken=${refreshToken}`)
    .then((result) => {
      console.log(result);
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

export const authService = {
  register,
  resend,
  activate,
  login,
  logout,
};
