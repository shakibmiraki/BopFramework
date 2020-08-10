/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { useTranslation } from "react-i18next";
import { StickyButton } from "../../components/Share/button";
import { Header } from "../../components/Share/header";
import { Formik, Form } from "formik";
import * as Yup from "yup";
import { CalendarPopup } from "../../components/Share/calendar-popup";
import { InputWithError, MaskedInputWithError } from "./../../components/Share/input";
import { GenderRadioWithError } from "./../../components/Share/gender";
import { useDispatch, useSelector } from "react-redux";
import { updateProfile, getProfile } from "../../actions/profile";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { mask } from "./../../constants/masks";
import { useEffect } from "react";
import { FaChevronRight } from "react-icons/fa";
import { Title, TextDirection } from "./../../components/Share/text";

const Schema = Yup.object().shape({
  first_name: Yup.string().required("share.first_name.validation.required"),
  last_name: Yup.string().required("share.last_name.validation.required"),
  email: Yup.string().email("share.validation.bad_format"),
  national_code: Yup.string()
    .matches(mask.validation.national_code, "share.validation.bad_format")
    .required("share.national_code.validation.required"),
  province: Yup.string().required("share.province.validation.required"),
  city: Yup.string().required("share.city.validation.required"),
  gender: Yup.string().required("share.gender.validation.required"),
  birth_date: Yup.string().required("share.birth_date.validation.required").nullable(),
});

const Profile = () => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { profile } = useSelector((state) => state.profile);

  useEffect(() => {
    dispatch(getProfile());
  }, []);

  return (
    <div className="h-100 d-flex flex-column" css={css(styles)}>
      <Header text={t("profile.title.profile")} burger back gray />

      <Zoom duration={config.animationDuration}>
        <div className="d-flex flex-grow-1">
          <Formik
            initialValues={{
              first_name: profile.firstName || "",
              last_name: profile.lastName || "",
              national_code: profile.nationalCode || "",
              birth_date: profile.birthDate,
              email: profile.email || "",
              gender: profile.gender.toString(),
            }}
            enableReinitialize={true}
            validationSchema={Schema}
            className="profile"
            onSubmit={(values, { resetForm }) => {
              dispatch(updateProfile(values));
            }}
          >
            {() => (
              <Form className="d-flex flex-column flex-grow-1">
                <div className="container flex-grow-1">
                  <TextDirection center>
                    <Title text={t("profile.title.set_profile")} />
                  </TextDirection>
                  <InputWithError
                    type="text"
                    name="first_name"
                    placeholder={t("share.text.first_name")}
                    autoComplete="off"
                  />

                  <InputWithError
                    type="text"
                    name="last_name"
                    placeholder={t("share.text.last_name")}
                    autoComplete="off"
                  />

                  <InputWithError
                    type="text"
                    name="email"
                    placeholder={t("share.text.email")}
                    autoComplete="off"
                  />

                  <MaskedInputWithError
                    name="national_code"
                    placeholder={t("share.text.national_code")}
                    placeholderChar="-"
                    mask={mask.national_code}
                    autoComplete="off"
                    inputMode="numeric"
                  />

                  <CalendarPopup name="birth_date" placeholder={t("share.text.birth_date")} />

                  <GenderRadioWithError />
                </div>

                <StickyButton type="submit">
                  <span>{t("share.button.save")}</span>
                  <i>{<FaChevronRight />}</i>
                </StickyButton>
              </Form>
            )}
          </Formik>
        </div>
      </Zoom>
    </div>
  );
};

export default Profile;
