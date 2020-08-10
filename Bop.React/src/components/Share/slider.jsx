/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import ItemsCarousel from "react-items-carousel";
import { useState } from "react";
import { FaAngleRight, FaAngleLeft } from "react-icons/fa";

const styles = css({
  padding: "0 20px",
  maxWidth: "1000px",
  margin: "0 auto",
  direction: "ltr",
  ".navigation-icon": {
    width: "30px",
    height: "30px",
    justifyContent: "center",
    alignItems: "center",
    display: "flex",
  },
});

export const Slider = ({ children }) => {
  const [activeItemIndex, setActiveItemIndex] = useState(0);

  return (
    <div css={styles}>
      <ItemsCarousel
        infiniteLoop={false}
        gutter={10}
        activePosition={"center"}
        chevronWidth={30}
        disableSwipe={false}
        alwaysShowChevrons={false}
        numberOfCards={2}
        slidesToScroll={2}
        outsideChevron={true}
        showSlither={true}
        firstAndLastGutter={false}
        activeItemIndex={activeItemIndex}
        requestToChangeActive={setActiveItemIndex}
        rightChevron={
          <div className="navigation-icon rounded-circle bg-white shadow-lg border">
            <FaAngleRight />
          </div>
        }
        leftChevron={
          <div className="navigation-icon rounded-circle bg-white shadow-lg border">
            <FaAngleLeft />
          </div>
        }
      >
        {children}
      </ItemsCarousel>
    </div>
  );
};
