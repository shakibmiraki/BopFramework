/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { useState } from "react";
import { Tab, Tabs, TabList, TabPanel } from "react-tabs";
import "react-tabs/style/react-tabs.scss";
import { useTranslation } from "react-i18next";
import { useSelector } from "react-redux";
import { localeService } from "../../services/locale";
import { GoHome } from "react-icons/go";
import { FaWallet, FaRegBookmark } from "react-icons/fa";
import { MenuTab } from "../Share/tab";
import { GrReactjs } from 'react-icons/gr';

const styles = css({
  ".react-tabs": {
    width: "100%",
  },
  // ".react-tabs .react-tabs__tab-panel": {
  //   display: "flex",
  //   position: "fixed",
  //   bottom: "55px",
  //   width: "100%",
  //   backgroundColor: "#9f46a3",
  //   color: "white",
  //   minHeight: "55px",
  //   zIndex: -1,
  // },
  ".react-tabs__tab-list": {
    display: "flex",
    borderBottom: "0",
    margin: "0",
    position: "fixed",
    bottom: "0",
    left: "0",
    right: "0",
    width: "100%",
    backgroundColor: "#f8f8f8",
    maxHeight: "55px",

    ".react-tabs__tab": {
      position: "relative",
      border: 0,
      borderBottom: 0,
      flex: "auto",
      textAlign: "center",
      padding: 0,
      ".tab-icon": {
        width: "50px",
        height: "50px",
        borderRadius: "50%",
        margin: "auto",
        fontSize: "30px",
        position: "relative",
        backgroundColor: "#f8f8f8",
        padding: "5px",
        transition: "all 0.3s",
      },
      ".tab-text": {
        opacity: 0,
        position: "relative",
        bottom: "25px",
        padding: 0,
        fontSize: "15px",
        transition: "all 0.3s",
      },
    },

    ".react-tabs__tab--selected": {
      background: "transparent",
      ".tab-icon": {
        transform: "translateY(-24px)",
        boxShadow: "0px -4px 6px -3px rgba(109, 109, 109, 0.3)",
      },
      ".tab-text": {
        opacity: 1,
        width: "100%",
      },
    },
  },
});

export const TabNavigation = ({ name }) => {
  const [tabIndex, setTabIndex] = useState(0);
  const { t } = useTranslation();
  const { ltr } = useSelector((state) => state.language);

  return (
    <div className="row" css={css(styles)} dir={`${localeService.getLocalDirectionName(ltr)}`}>
      <Tabs
        direction={`${localeService.getLocalDirectionName(ltr)}`}
        selectedIndex={tabIndex}
        onSelect={(tabIndex) => setTabIndex(tabIndex)}
      >
        <TabList>
          <Tab>
            <MenuTab active={tabIndex === 0}>
              <div className="tab-icon">
                <GoHome />
              </div>
              <div className="tab-text">{t("tab_navigation.menu.title.home")}</div>
            </MenuTab>
          </Tab>
          <Tab>
            <MenuTab active={tabIndex === 1}>
              <div className="tab-icon">
                <FaWallet />
              </div>
              <div className="tab-text">{t("tab_navigation.menu.title.my_cards")}</div>
            </MenuTab>
          </Tab>
          <Tab>
            <MenuTab active={tabIndex === 2}>
              <div className="tab-icon">
                <FaRegBookmark />
              </div>
              <div className="tab-text">{t("tab_navigation.menu.title.favorite_pages")}</div>
            </MenuTab>
          </Tab>
          <Tab>
            <MenuTab active={tabIndex === 3}>
              <div className="tab-icon">
                <GrReactjs />
              </div>
              <div className="tab-text">{t("tab_navigation.menu.title.bop")}</div>
            </MenuTab>
          </Tab>
        </TabList>

        <TabPanel></TabPanel>
        <TabPanel></TabPanel>
        <TabPanel></TabPanel>
        <TabPanel></TabPanel>
      </Tabs>
    </div>
  );
};
