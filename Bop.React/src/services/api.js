import axios from "axios";
import promise from "promise";
import { tokenService } from "./token";

// Add a request interceptor
export const api = axios.create();

api.interceptors.request.use(
  async function (config) {
    //if token is found add it to the header
    config.headers["Accept-Language"] = "fa-IR";
    const token = tokenService.getJwtToken();
    if (token) {
      config.headers["Authorization"] = `Bearer ${tokenService.getJwtToken()}`;
    }
    return config;
  },
  function (error) {
    // Do something with request error
    return promise.reject(error);
  }
);
