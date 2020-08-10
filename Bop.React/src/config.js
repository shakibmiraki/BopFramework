import { themes } from "./constants/theme";

export const config = {
  apiUrl: process.env.NODE_ENV === "production" ? "http://localhost:5000" : "http://localhost:5000",
  animationDuration: 400,
  defaultTheme: themes.maroon.name,
  os: {
    name: "PWA",
    value: "1",
  },
  app_version: "0.1",
  admin_role_name: "Administrator",
  toast_auto_close: 5000,
};
