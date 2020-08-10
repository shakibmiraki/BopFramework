/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { Field } from "formik";
import MaskedInput from "react-text-mask";
import { ErrorMessage } from "formik";
import { SolidButton } from "./button";
import { PrimaryText } from "./text";
import { useSelector } from "react-redux";
import { mask } from "../../constants/masks";
import createAutoCorrectedDatePipe from "text-mask-addons/dist/createAutoCorrectedDatePipe";
import { useTranslation } from "react-i18next";
import Countdown, { zeroPad } from "react-countdown";
import { useState } from "react";
import { useFormikContext } from "formik";
import i18next from "i18next";
import { toastService } from "../../services/toast";
import { utilService } from "../../services/utils";
import { global_config } from "../../constants/constant";

export const InputWithError = ({ type, name, placeholder, className, autoComplete, icon, inputMode }) => {
  const { t } = useTranslation();
  return (
    <div className="form-group position-relative">
      <Field
        className={`form-control shadow-sm rounded-pill ${className}
        ${icon ? "pl-5" : ""}
        `}
        type={type}
        name={name}
        placeholder={placeholder}
        autoComplete={autoComplete}
        inputMode={inputMode}
      />
      {icon}
      <ErrorMessage name={name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
    </div>
  );
};

export const OtpInputWithError = ({ pin2_fieldname, placeholder, text, onGetOtp }) => {
  const [showOtp, setShowOtp] = useState(true);
  const { values, setFieldValue } = useFormikContext();
  const { t } = useTranslation();
  const { ltr } = useSelector((state) => state.language);

  const renderer = ({ minutes, seconds }) => {
    // Render a countdown
    return (
      <span
        // className="form-control"
        className={`btn btn-block d-inline-block`}
        css={css(
          ltr
            ? {
                borderTopLeftRadius: "0 !important",
                borderBottomLeftRadius: "0 !important",
                borderRadius: global_config.border_radius,
                width: "33%",
                backgroundColor: "#868686",
                color: "white",
                justifyContent: "center",
                alignItems: "center",
                padding: "9px 7px !important",
              }
            : {
                borderTopRightRadius: "0 !important",
                borderBottomRightRadius: "0 !important",
                borderRadius: global_config.border_radius,
                width: "33%",
                backgroundColor: "#868686",
                color: "white",
                justifyContent: "center",
                alignItems: "center",
                padding: "9px 7px !important",
              }
        )}
      >
        {zeroPad(minutes)}:{zeroPad(seconds)}
      </span>
    );
  };

  return (
    <div className="form-group position-relative row">
      <div className="col-12">
        <div className="d-inline-block" css={css({ width: "67%" })}>
          <div className="form-group position-relative">
            <Field
              className="form-control shadow-sm rounded-pill text-center"
              css={css(
                ltr
                  ? {
                      borderTopRightRadius: "0 !important",
                      borderBottomRightRadius: "0 !important",
                    }
                  : { borderTopLeftRadius: "0 !important", borderBottomLeftRadius: "0 !important" }
              )}
              type="password"
              name={pin2_fieldname}
              placeholder={placeholder}
              inputMode="numeric"
              autoComplete="one-time-code"
            />
          </div>
        </div>

        {showOtp ? (
          <SolidButton
            type="button"
            onClick={() => {
              if (values.pan) {
                onGetOtp();
                setShowOtp(false);
                setFieldValue("timer", utilService.getOtpTimer());
              } else {
                toastService.notify(i18next.t("payment.validation.pan.required"));
              }
            }}
            className={`btn btn-block d-inline-block`}
            css={css(
              ltr
                ? {
                    borderTopLeftRadius: "0 !important",
                    borderBottomLeftRadius: "0 !important",
                    borderRadius: global_config.border_radius,
                    width: "33%",
                    display: "inline-block",
                    fontSize: "14px",
                  }
                : {
                    borderTopRightRadius: "0 !important",
                    borderBottomRightRadius: "0 !important",
                    borderRadius: global_config.border_radius,
                    width: "33%",
                    display: "inline-block",
                    fontSize: "14px",
                  }
            )}
          >
            {text}
          </SolidButton>
        ) : (
          <Countdown
            autoStart={true}
            date={values.timer}
            onStart={() => setShowOtp(false)}
            onComplete={() => setShowOtp(true)}
            renderer={renderer}
          ></Countdown>
        )}
        <ErrorMessage name={pin2_fieldname} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
      </div>
    </div>
  );
};

export const MaskedInputWithError = ({
  name,
  placeholder,
  placeholderChar,
  mask,
  guide = true,
  className,
  labelText,
  autoComplete,
  icon,
  inputMode,
}) => {
  const { t } = useTranslation();

  return (
    <div className="form-group position-relative">
      {labelText ? (
        <label htmlFor={`${name}`}>
          <PrimaryText>{labelText}</PrimaryText>
        </label>
      ) : null}
      <Field name={`${name}`}>
        {({ field }) => (
          <MaskedInput
            css={css({ direction: "ltr" })}
            className={`form-control shadow-sm rounded-pill ${className}
            ${icon ? "pl-5" : ""}
            `}
            placeholder={placeholder}
            mask={mask}
            placeholderChar={placeholderChar}
            guide={guide}
            autoComplete={autoComplete}
            {...field}
            inputMode={inputMode}
          />
        )}
      </Field>
      {icon}
      <ErrorMessage name={name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
    </div>
  );
};

export const MaskedCurrencyWithError = ({ name, placeholder, guide = true, className, labelText }) => {
  const { t } = useTranslation();

  return (
    <div className="form-group">
      {labelText ? (
        <label htmlFor={`${name}`}>
          <PrimaryText>{labelText}</PrimaryText>
        </label>
      ) : null}
      <Field name={`${name}`}>
        {({ field }) => (
          <MaskedInput
            className={`form-control shadow-sm rounded-pill ${className}`}
            placeholder={placeholder}
            mask={mask.currencyMask}
            placeholderChar="-"
            autoComplete="off"
            guide={guide}
            {...field}
            inputMode="numeric"
          />
        )}
      </Field>
      <ErrorMessage name={name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
    </div>
  );
};

export const MaskedExpireDateWithError = ({
  name,
  placeholder,
  guide = true,
  className,
  labelText,
  autoComplete,
}) => {
  const { t } = useTranslation();
  return (
    <div>
      <div className="form-group">
        {labelText ? (
          <label htmlFor={`${name}`}>
            <PrimaryText>{labelText}</PrimaryText>
          </label>
        ) : null}
        <Field name={`${name}`}>
          {({ field }) => (
            <MaskedInput
              className={`form-control shadow-sm rounded-pill ${className}`}
              placeholder={placeholder}
              mask={mask.expire_date}
              pipe={createAutoCorrectedDatePipe("yy/mm")}
              placeholderChar="-"
              guide={guide}
              autoComplete={autoComplete}
              {...field}
              inputMode="numeric"
            />
          )}
        </Field>
      </div>
      <ErrorMessage name={name} render={(msg) => <div className="text-danger">{t(msg)}</div>} />
    </div>
  );
};
