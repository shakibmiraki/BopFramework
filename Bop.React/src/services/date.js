import moment from "moment-jalaali";

const unixNow = () => {
  const date = Date.now();
  return date;
};

const JalalitoUnix = (shamsiDate) => {
  const gregorian = moment(shamsiDate, "jYYYY/jM/jD").unix();
  return gregorian;
};

const unixToJalaliJson = (unix_date) => {
  let date = new Date(unix_date * 1000);
  const georgianDate = moment(date).format("YYYY-MM-DD");
  return {
    year: parseInt(moment(georgianDate, "YYYY-MM-DD").format("jYYYY")),
    month: parseInt(moment(georgianDate, "YYYY-MM-DD").format("jM")),
    day: parseInt(moment(georgianDate, "YYYY-MM-DD").format("jD")),
  };
};

const unixToJalaliString = (unix_date) => {
  let date = new Date(unix_date * 1000);
  return moment(date).format("jYYYY/jM/jD");
};

const formatDateWithSeparator = (date) => {
  const year = date.substring(0, 4);
  const month = date.substring(4, 6);
  const day = date.substring(6, 8);
  return `${year}/${month}/${day}`; //13990405 ---> 1399/04/05
};

const jsonToString = (json_date) => {
  if (json_date) {
    const jalali_date = `${json_date.year}/${json_date.month}/${json_date.day}`;
    moment.loadPersian({ usePersianDigits: true });
    const date = moment(jalali_date, "jYYYY/jM/jD");
    return `${date.format("jDD")} ${date.format("jMMMM")} ${date.format("jYYYY")}`;
  } else {
    return null;
  }
};

const getCurrentDateTime = () => {
  const date_object = new Date();
  const date_jalali = new Intl.DateTimeFormat("fa").format(Date.now());
  const date_hour = date_object.getHours();
  const date_minutes = date_object.getMinutes();
  const date_second = date_object.getSeconds();
  const dateString = `${date_jalali}-${date_hour}:${date_minutes}:${date_second}`;
  return {
    date: date_jalali,
    hour: date_hour,
    minute: date_minutes,
    second: date_second,
    appended: dateString,
  };
};

const georgianToJalali = (miladi, georgianFormat, jalaliFormat, usePersianDigits = false) => {
  moment.loadPersian({ usePersianDigits: usePersianDigits, dialect: "persian-modern" });
  const jalaliDate = moment(miladi, georgianFormat).format(jalaliFormat);
  return jalaliDate;
};

export const dateService = {
  JalalitoUnix,
  unixToJalaliJson,
  unixToJalaliString,
  formatDateWithSeparator,
  jsonToString,
  getCurrentDateTime,
  unixNow,
  georgianToJalali,
};
