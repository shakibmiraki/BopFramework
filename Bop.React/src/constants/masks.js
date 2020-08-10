import createNumberMask from "text-mask-addons/dist/createNumberMask";

export const mask = {
  mobile: ["+", "9", "8", "-", /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/],
  phone: [/\d/, /\d/, /\d/, "-", /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/],
  phone_code: [/\d/, /\d/, /\d/],
  expire_date: [/\d/, /\d/, "/", /\d/, /\d/],
  pan_mask: [
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    " ",
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    " ",
    /\d/,
    /\d/,
    /\d/,
    /\d/,
    " ",
    /\d/,
    /\d/,
    /\d/,
    /\d/,
  ],
  national_code: [/\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/, /\d/],
  two_digit: [/\d/, /\d/],
  three_digit: [/\d/, /\d/, /\d/],
  persian_character: [/^[\u0600-\u06FF\s]+$/],
  currencyMask: createNumberMask({
    ...{
      prefix: "",
      suffix: "",
      includeThousandsSeparator: true,
      thousandsSeparatorSymbol: ",",
      allowDecimal: true,
      decimalSymbol: ".",
      decimalLimit: 2, // how many digits allowed after the decimal
      integerLimit: 10, // limit length of integer numbers
      allowNegative: false,
      allowLeadingZeroes: false,
    },
  }),
  validation: {
    mobile: /^[+][9][8][-][9][0-9]{9}$/,
    phone: /^[0-9]{3}[-][0-9]{8}$/,
    activation_code: /^[\d][\d][\d][\d][\d]$/,
    expire_date: /^\d{2}\/\d{2}$/,
    national_code: /^[0-9]{10}$/,
  },
};
