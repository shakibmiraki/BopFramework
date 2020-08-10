import { api } from "./api";
import { config } from "./../config";
import { utilService } from "./utils";
import { storage_key } from "../constants/constant";
import { localStorageService } from "./localStorage";
import { userService } from "./user";

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

const setSignedUp = () => {
  localStorageService.setKey(storage_key.user_signed_up, true);
  return Promise.resolve();
};

const getSignedUp = () => {
  return localStorageService.getKey(storage_key.user_signed_up);
};

const setActivationSent = () => {
  localStorageService.setKey(storage_key.activation_send, true);
  return Promise.resolve();
};

const getActivationSent = () => {
  return localStorageService.getKey(storage_key.activation_send);
};

export const authService = {
  register,
  resend,
  activate,
  login,
  setSignedUp,
  getSignedUp,
  setActivationSent,
  getActivationSent,
};
