import { colors } from "./colors";

export const themes = {
  maroon: {
    name: "maroon",
    primaryColor: colors.maroon_dark,
    gradient: [colors.maroon_dark, colors.maroon_light, colors.white],
    accentColor: colors.blue,
    backgroundColor: colors.white,
    textColor: colors.maroon_dark,
    secondaryTextColor: colors.gray_light_2,
  },
  green: {
    name: "green",
    primaryColor: colors.green_dark,
    gradient: [colors.green_dark, colors.green_light, colors.white],
    accentColor: colors.blue,
    backgroundColor: colors.white,
    textColor: colors.green_dark,
    secondaryTextColor: colors.gray_light_2,
  },
  purple: {
    name: "purple",
    primaryColor: colors.purple_dark,
    gradient: [colors.purple_dark, colors.purple_light, colors.white],
    accentColor: colors.blue,
    backgroundColor: colors.white,
    textColor: colors.purple_dark,
    secondaryTextColor: colors.gray_light_2,
  },
};
