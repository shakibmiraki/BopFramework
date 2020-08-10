import * as types from "../actionTypes/profile";
import { DEFAULT_KEY, generateCacheTTL } from "redux-cache";

const initialState = {
  [DEFAULT_KEY]: null,
  profile: {
    email: "",
    firstName: "",
    gender: "",
    lastName: "",
    mobile: "",
    nationalCode: "",
    province_Id: "",
    contacts: [],
    cards: [],
    transactions: [],
  },
  cards: [],
  provinces: [],
  cities: [],
};

export default function profileReducer(state = initialState, action) {
  if (action.type === types.ProfileFetched) {
    return {
      ...state,
      profile: {
        email: action.payload.email,
        firstName: action.payload.firstName,
        gender: action.payload.gender,
        lastName: action.payload.lastName,
        mobile: action.payload.mobile,
        nationalCode: action.payload.nationalCode,
        province_Id: action.payload.province_Id,
        city_Id: action.payload.city_Id,
        birthDate: action.payload.birthDate,
        contacts: action.payload.contacts,
        transactions: action.payload.transactions,
      },
    };
  }

  if (action.type === types.CardsFetched) {
    return {
      ...state,
      cards: action.payload,
    };
  }
  if (action.type === types.ProvinceFetched) {
    return {
      ...state,
      [DEFAULT_KEY]: generateCacheTTL(),
      provinces: action.payload,
    };
  }
  if (action.type === types.CityFetched) {
    return {
      ...state,
      cities: action.payload,
    };
  }

  return state;
}
