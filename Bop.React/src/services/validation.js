import { mask } from "../constants/masks";
import i18next from "i18next";

const validateMobile = (value) => {
  let error;
  if (!mask.validation.mobile.test(value)) {
    error = i18next.t("share.validation.mobile.bad_format");
  }
  return error;
};

const validatePhone = (value) => {
  let error;
  if (!mask.validation.phone.test(value)) {
    error = i18next.t("share.validation.phone.bad_format");
  }
  return error;
};

const validatePan = (value) => {
  let error;
  if (!mask.validation.pan.test(value)) {
    error = i18next.t("share.validation.pan.bad_format");
  }
  return error;
};

export const validationService = {
  validateMobile,
  validatePhone,
  validatePan,
};
