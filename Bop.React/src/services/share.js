const appendText = (...params) => {
  let text = "";
  params.forEach((item, index) => {
    text += item + "\n";
  });
  return text;
};

const ShareAsText = (title, text) => {
  if (navigator.share) {
    navigator
      .share({
        title: title,
        text: text,
      })
      .then(
        () => {},
        (error) => {
          console.log("Error sharing:", error);
        }
      );
  } else {
    console.log(`Your system doesn't support sharing files.`);
  }
};

// const toImage = () => {
//   let node = document.getElementById("share-as-image");
//   domtoimage
//     .toPng(node)
//     .then(function (dataUrl) {
//       // console.log(dataUrl);
//       console.log(dataUrl);
//       if (navigator.share) {
//         navigator
//           .share({
//             title: "رسید ایران کیش",
//             url: dataUrl,
//             mimeType: "image/png",
//           })
//           .then(
//             () => {},
//             (error) => {
//               console.log("Error sharing:", error);
//             }
//           );
//       } else {
//         console.log(`Your system doesn't support sharing files.`);
//       }
//     })
//     .catch(function (error) {
//       console.error("oops, something went wrong!", error);
//     });
// };


export const shareService = {
  appendText,
  ShareAsText
};
