import * as types from "../actionTypes/app";
import { appService } from "./../services/app";
import { isRequesting, isRequested, errorOccurred } from "./request";

export const menuFetched = (categories) => ({
  type: types.MenuFetched,
  payload: categories,
});

export const fetchMenu = () => (dispatch) => {
  dispatch(isRequesting());
  appService
    .fetchMenu()
    .then(async (result) => {
      await dispatch(menuFetched(result.data.data.categories));
      dispatch(isRequested());
    })
    .catch((error) => {
      dispatch(errorOccurred());
    });
};
