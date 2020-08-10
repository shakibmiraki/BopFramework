import { localStorageService } from "./localStorage";
import { storage_key } from "../constants/constant";
import * as jwt_decode from "jwt-decode";

const setJwtToken = (token) => {
  localStorageService.setKey(storage_key.jwt_token, token);
  return Promise.resolve();
};

const getJwtToken = () => {
  return localStorageService.getKey(storage_key.jwt_token);
};

const setRefreshToken = (refresh_token) => {
  localStorageService.setKey(storage_key.refresh_token, refresh_token);
  return Promise.resolve();
};

const getRefreshToken = () => {
  return localStorageService.getKey(storage_key.refresh_token);
};

const getAccessTokenExpirationDateUtc = () => {
  const decoded = getDecodedAccessToken();
  if (decoded.exp === undefined) {
    return null;
  }
  const date = new Date(0); // The 0 sets the date to the epoch
  date.setUTCSeconds(decoded.exp);
  return date;
};

const hasAccessTokenExpired = () => {
  console.log("x");
  const expirationDateUtc = getAccessTokenExpirationDateUtc();
  if (!expirationDateUtc) {
    return true;
  }
  return !(expirationDateUtc.valueOf() > new Date().valueOf());
};

const getDecodedAccessToken = () => {
  return jwt_decode(getJwtToken());
};

const getDecodedTokenRoles = () => {
  const decodedToken = getDecodedAccessToken();
  const roles = decodedToken["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  if (!roles) {
    return null;
  }

  if (Array.isArray(roles)) {
    return roles.map((role) => role.toLowerCase());
  } else {
    return [roles.toLowerCase()];
  }
};

export const tokenService = {
  setJwtToken,
  getJwtToken,
  setRefreshToken,
  getRefreshToken,
  hasAccessTokenExpired,
  getDecodedAccessToken,
  getDecodedTokenRoles,
};
