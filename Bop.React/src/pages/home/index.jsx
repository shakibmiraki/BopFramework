/* eslint-disable react-hooks/exhaustive-deps */
/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import styles from "./index-style";
import { useTranslation } from "react-i18next";
import { Header } from "./../../components/Share/header";
import { MenuCircle } from "../../components/Menu/menu-circle";
import Zoom from "react-reveal/Zoom";
import { config } from "../../config";
import { TabNavigation } from "./../../components/Menu/tab-navigation";
import { Title, TextDirection } from "./../../components/Share/text";
import { routes } from "../../constants/constant";
import { FcBriefcase, FcClearFilters, FcCapacitor, FcGallery, FcLike, FcPrivacy, FcWebcam, FcPhotoReel, FcMultipleCameras } from "react-icons/fc";

const Home = () => {
  const { t } = useTranslation();

  return (
    <div css={css(styles)}>
      <Header logo burger />
      <div className="container">
        <div className="home">
          <div className="menu-container">
            <Zoom duration={config.animationDuration}>
              <TextDirection center className="mt-3">
                <Title text={t("home.menu.header.popular_service")} gray padding="p-0" />
              </TextDirection>
              <div className="row mb-3">
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcBriefcase fontSize="3rem" />}
                />

                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcMultipleCameras fontSize="3rem" />}
                />

                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcPhotoReel fontSize="3rem" />}
                />
              </div>
            </Zoom>

            <Zoom duration={config.animationDuration}>
              <TextDirection center>
                <Title text={t("home.menu.header.service")} gray padding="p-0" />
              </TextDirection>
              <div className="row">
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcWebcam fontSize="3rem" />}
                />
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcPrivacy fontSize="3rem" />}
                />
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcLike fontSize="3rem" />}
                />
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcGallery fontSize="3rem" />}
                />
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcCapacitor fontSize="3rem" />}
                />
                <MenuCircle
                  url={routes.home.base}
                  text={t("share.menu.title.menu_title")}
                  icon={<FcClearFilters fontSize="3rem" />}
                />
              </div>
            </Zoom>
          </div>
          <TabNavigation />
        </div>
      </div>
    </div>
  );
};

export default Home;
