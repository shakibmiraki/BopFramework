/** @jsx jsx */
import { jsx, css } from "@emotion/core";
import { images } from "./../../constants/images";


// export const SquaredBackground = ({ rows = 10, columns = 22, color = "#efefef" }) => {
//   let skewX = 0;
//   let skewY = 0;
//   return (
//     <div
//       className="fluid-container square-background"
//       style={{
//         position: "absolute",
//         height: "65%",
//         top: 0,
//         right: 0,
//         left: 0,
//         overflow: "hidden",
//         width: "100%",
//         zIndex: "-1",
//       }}
//     >
//       {Array.apply(0, Array(rows)).map(function (x, i) {
//         let base = 50;
//         let xValue = (base - columns) * 0.1;
//         let yValue = (base - columns) * 0.2;
//         skewX = skewX - xValue;
//         skewY = skewY - yValue;
//         let columnWidth = `${100 / columns}%`;
//         return (
//           <div className="row" key={i}>
//             {Array.apply(0, Array(columns)).map(function (x, i) {
//               return (
//                 <div
//                   key={i}
//                   style={{
//                     width: columnWidth,
//                     height: "25px",
//                     padding: "1px",
//                     float: "right",
//                     transform: `skew(${skewX}deg, ${skewY}deg)`,
//                   }}
//                 >
//                   <div
//                     style={{
//                       backgroundColor: color,
//                       width: "100%",
//                       height: "100%",
//                     }}
//                   ></div>
//                 </div>
//               );
//             })}
//           </div>
//         );
//       })}
//     </div>
//   );
// };

export const SquaredBackground = () => {
  return (
    <div className="square-background h-100">
      <img
        css={css({ width: "100%", height: "100%" })}
        src={images.background_pattern}
        alt="put background pattern"
      />
    </div>
  );
};

export const CardBackgroundPattern = ({ rows, columns, color = "#b7b7b7" }) => {
  return (
    <div className="container" style={{ position: "absolute", top: 0, right: 0 }}>
      {Array.apply(0, Array(rows)).map(function (x, i) {
        let skewX = 0;
        let skewY = 0;
        let base = 50;
        let xValue = (base - columns) * 0.12;
        let yValue = (base - columns) * 0.07;
        return (
          <div className="row" key={i}>
            {Array.apply(0, Array(columns)).map(function (x, i) {
              skewX = skewX - xValue;
              skewY = skewY - yValue;
              return (
                <div
                  key={i}
                  style={{
                    width: "15px",
                    height: "15px",
                    backgroundColor: color,
                    float: "right",
                    margin: "1px",
                    transform: `skew(${skewX}deg, ${skewY}deg)`,
                  }}
                ></div>
              );
            })}
          </div>
        );
      })}
    </div>
  );
};
