/* eslint-disable no-unused-expressions */
import { api } from "./api";
import { config } from "./../config";
import { utilService } from "./utils";
import { localStorageService } from "./localStorage";
import { storage_key } from "../constants/constant";

const getProfile = async () => {
  return api
    .get(`${config.apiUrl}/api/customer/getprofile`)
    .then((result) => {
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const updateProfile = async (model) => {
  const value = {
    firstName: model.first_name,
    lastName: model.last_name,
    email: model.email,
    nationalCode: model.national_code,
    birthDate: model.birth_date, //"0001-01-01T00:00:00"
    gender: model.gender,
  };
  return api
    .post(`${config.apiUrl}/api/customer/updateprofile`, value)
    .then((result) => {
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

const setAvatar = (file) => {
  localStorageService.setKey(storage_key.profile_avatar, file);
};

const getAvatar = () => {
  return localStorageService.getKey(storage_key.profile_avatar);
};

export const profileService = {
  getProfile,
  updateProfile,
  setAvatar,
  getAvatar,
};
