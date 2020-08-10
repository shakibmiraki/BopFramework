export const global_config = {
  font_name: "Vazir",
  font_size: "14px",
  circle_fix_width: "105px",
  card_max_width: "350px",
  border_radius: "12px",
};

export const routes = {
  root: "/",
  splash: "/splash",
  language: "/language",
  home: "/home",
  sign_up: {
    base: "/signup",
    activation: "/activation",
  },
  login: {
    base: "/login",
  },
  profile: "/profile",

  about_us: "/about-us",
  not_found: "/not-found",
};

const storage_key_prefix = "bop_";
export const storage_key = {
  mobile: `${storage_key_prefix}mobile`,
  representer_mobile: `${storage_key_prefix}representer_mobile`,
  user_signed_up: `${storage_key_prefix}user_signed_up`,
  activation_send: `${storage_key_prefix}activation_send`,
  theme_name: `${storage_key_prefix}ikccc_theme`,
  theme_initialize: `${storage_key_prefix}theme_initialize`,
  user_registered: `${storage_key_prefix}user_registered`,
  i18nextLng: "i18nextLng",
  language_initialize: `${storage_key_prefix}language_initialize`,
  profile_avatar: `${storage_key_prefix}profile_avatar`,
  jwt_token: `${storage_key_prefix}jwt_token`,
  refresh_token: `${storage_key_prefix}refresh_token`,
};
