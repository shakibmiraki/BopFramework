import { localeService } from "../services/locale";
import { themeService } from "../services/theme";
import { global_config } from "./constant";

const customStyles = {
  control: (provided) => ({
    ...provided,
    borderRadius: global_config.border_radius,
    minHeight: "44px",
    flexDirection: localeService.isLTR() ? "row" : "row-reverse",
  }),
  menu: (provided) => ({ ...provided, overflow: "hidden", padding: 0 }),
  menuList: (provided) => ({ ...provided, padding: 0, lineHeight: "17px", fontSize: "14px", textAlign: "center" }),
  singleValue: (provided) => ({ ...provided, fontSize: "13px" }),
  indicatorSeparator: (provided) => ({}),
  indicatorsContainer: (provided) => ({}),
  dropdownIndicator: (provided) => ({ ...provided, color: themeService.getTheme().primaryColor }),
};

const pelakCustomStyles = {
  control: (provided) => ({
    ...provided,
    borderRadius: global_config.border_radius,
    minHeight: "44px",
    flexDirection: localeService.isLTR() ? "row" : "row-reverse",
    borderWidth:"0",
    boxShadow:"unset !important"
  }),
  menu: (provided) => ({ ...provided, overflow: "hidden", padding: 0 }),
  menuList: (provided) => ({ ...provided, padding: 0, lineHeight: "17px", fontSize: "14px", textAlign: "center" }),
  singleValue: (provided) => ({ ...provided, fontSize: "13px" }),
  indicatorSeparator: (provided) => ({}),
  indicatorsContainer: (provided) => ({}),
  dropdownIndicator: (provided) => ({ ...provided, color: themeService.getTheme().primaryColor }),
};

export const selectConfig = {
  customStyles,
  pelakCustomStyles,
};
