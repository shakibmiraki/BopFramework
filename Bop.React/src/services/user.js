import { localStorageService } from "./localStorage";
import { storage_key } from "../constants/constant";
import { tokenService } from "./token";
import { config } from "./../config";

function setMobile(mobile) {
  localStorageService.setKey(storage_key.mobile, mobile);
  return Promise.resolve();
}

function getMobile() {
  return localStorageService.getKey(storage_key.mobile);
}

function setRepresenterMobile(representer_mobile) {
  localStorageService.setKey(storage_key.representer_mobile, representer_mobile);
  return Promise.resolve();
}

function getRepresenterMobile() {
  return localStorageService.getKey(storage_key.representer_mobile);
}

const getAuthUser = () => {
  if (!isAuthUserLoggedIn()) {
    return null;
  }

  const decodedToken = tokenService.getDecodedAccessToken();
  const roles = tokenService.getDecodedTokenRoles();
  return Object.freeze({
    userId: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
    userName: decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
    displayName: decodedToken["DisplayName"],
    roles: roles,
  });
};

const isAuthUserLoggedIn = () => {
  const accessToken = tokenService.getJwtToken();
  const refreshToken = tokenService.getRefreshToken();
  return accessToken && refreshToken && !tokenService.hasAccessTokenExpired();
};

const isAuthUserInRoles = (requiredRoles) => {
  const user = getAuthUser();

  if (!user || !user.roles) {
    return false;
  }

  if (user.roles.indexOf(config.admin_role_name.toLowerCase()) >= 0) {
    return true; // The `Admin` role has full access to every pages.
  }

  return requiredRoles.some((requiredRole) => {
    if (user.roles) {
      return user.roles.indexOf(requiredRole.toLowerCase()) >= 0;
    } else {
      return false;
    }
  });
};

export const userService = {
  setMobile,
  getMobile,
  setRepresenterMobile,
  getRepresenterMobile,
  getAuthUser,
  isAuthUserLoggedIn,
  isAuthUserInRoles,
};
