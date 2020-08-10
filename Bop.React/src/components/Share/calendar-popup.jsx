/** @jsx jsx */
import { jsx } from "@emotion/core";
import "react-modern-calendar-datepicker/lib/DatePicker.css";
import { useSelector } from "react-redux";
import { Calendar } from "react-modern-calendar-datepicker";
import { useState, useEffect } from "react";
import ReactModal from "react-modal";
import { Button } from "./button";
import { useTranslation } from "react-i18next";
import { themeService } from "../../services/theme";
import { ErrorMessage, useFormikContext } from "formik";
import { Field } from "formik";
import { popupConfig } from "../../constants/popup-config";

export const CalendarPopup = ({ name, placeholder }) => {
  ReactModal.setAppElement("#root");

  const formik = useFormikContext();
  const field = formik.getFieldProps(name);

  const { locale } = useSelector((state) => state.language);
  const [selectedDate, setSelectedDate] = useState(field.value || "");

  const theme = themeService.getTheme();
  const [showModal, setShowModal] = useState(false);
  const { t } = useTranslation();

  useEffect(() => {
    const date_value = field.value ? `${field.value.year}/${field.value.month}/${field.value.day}` : "";
    setSelectedDate(date_value);
  }, [field.value]);

  return (
    <div className="form-group">
      <Field
        className="form-control shadow-sm rounded-pill"
        type="text"
        placeholder={placeholder}
        onClick={() => setShowModal(true)}
        value={selectedDate}
        autoComplete="off"
      />
      <ErrorMessage name={name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />

      <ReactModal
        isOpen={showModal}
        contentLabel="onRequestClose Example"
        onRequestClose={() => setShowModal(false)}
        style={popupConfig.customStyles}
      >
        <Calendar
          name={name}
          value={field.value}
          onChange={(value) => {
            formik.setFieldValue(name, value);
          }}
          colorPrimary={theme.primaryColor}
          shouldHighlightWeekends
          locale={locale}
        />
        <div className="mt-3">
          <Button type="button" onClick={() => setShowModal(false)} text={t("share.button.continue")} />
        </div>
      </ReactModal>
    </div>
  );
};
