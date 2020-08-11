import * as types from "../actionTypes/auth";
import { authService } from "../services/auth";
import { routes } from "../constants/constant";
import history from "./../services/history";
import { isRequesting, isRequested, errorOccurred } from "./request";
import { userService } from "./../services/user";
import { tokenService } from "../services/token";

export const setStep = (step) => ({
  type: types.SetStep,
  payload: step,
});

export const register = (model) => (dispatch) => {
  dispatch(isRequesting());
  return authService
    .register(model)
    .then((result) => {
      console.log(result);
      userService.setMobile(model.mobile);
      dispatch(isRequested(result.data.messages));
      history.push(`${routes.sign_up.base}${routes.sign_up.activation}`);
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};

export const resend = (model) => (dispatch) => {
  dispatch(isRequesting());
  return authService
    .resend(model)
    .then((result) => {
      dispatch(isRequested(result.data.messages));
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};

export const activate = (model) => (dispatch) => {
  dispatch(isRequesting());
  authService
    .activate(model)
    .then(async (result) => {
      console.log(result);
      dispatch(isRequested());
      history.push(`${routes.login.base}`);
    })
    .catch((error) => {
      console.log(error.response);
      dispatch(errorOccurred());
    });
};

export const login = (model) => (dispatch) => {
  console.log(model);
  dispatch(isRequesting());
  return authService
    .login(model)
    .then((result) => {
      console.log(result);
      tokenService.setJwtToken(result.data.accessToken);
      tokenService.setRefreshToken(result.data.refreshToken);
      dispatch(isRequested());
      history.push(`${routes.home}`);
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};

export const logout = () => (dispatch) => {
  dispatch(isRequesting());
  return authService
    .logout()
    .then(() => {
      tokenService.removeJwtToken();
      tokenService.removeRefreshToken();
      dispatch(isRequested());
      history.push(`${routes.login.base}`);
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};
