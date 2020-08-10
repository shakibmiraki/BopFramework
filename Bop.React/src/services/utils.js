/* eslint-disable no-control-regex */
import "clientjs";
import { toastService } from "./toast";
import i18next from "i18next";
import { config } from "./../config";

const generic_message = () => {
  return i18next.t("share.error.network_access_error");
};

const formatActivationCode = (date) => {
  return date.replace(/[ ]/g, "");
};

const formatNumber = (number) => {
  if (number) {
    return number.toString().replace(/[٬]/g, "").replace(/[,]/g, "");
  }
};

const getOSName = () => {
  // let client = new ClientJS();
  // return client.getOS().name;
  return config.os.value;
};

const getUniqueId = () => {
  // eslint-disable-next-line no-undef
  let client = new ClientJS();
  return client.getFingerprint();
};

const getBrowser = () => {
  // eslint-disable-next-line no-undef
  let client = new ClientJS();
  return client.getBrowser();
};

const getIpAddress = () => {
  return "192.168.0.1";
};

const getRandomDigit = () => {
  return Date.now();
};

function isEmpty(obj) {
  return Object.keys(obj).length === 0;
}

const handleError = (error) => {
  console.log(error);
  console.log(error.response);
  if (!error || !error.response || !error.response.data) {
    toastService.notify(generic_message());
  } else {
    toastService.notifies(error.response.data.messages);
  }
};

const getActivationSmsTimer = () => {
  return Date.now() + 60000;
};

const normalizeMobile = (mobile) => {
  if (!mobile) return;
  if (mobile.length === 14) {
    mobile = mobile.replace(/[+]/g, "").replace(/[-]/g, "");
    mobile = mobile.substring(2);
    return `0${mobile}`; //+98-9185198393 --> 09185198393
  } else if (mobile.length === 12) {
    mobile = mobile.substring(2);
    return `0${mobile}`; //989185198393 --> 09185198393
  }
};

export const utilService = {
  formatActivationCode,
  formatNumber,
  getOSName,
  getUniqueId,
  getBrowser,
  getIpAddress,
  getRandomDigit,
  isEmpty,
  handleError,
  getActivationSmsTimer,
  normalizeMobile,
};
