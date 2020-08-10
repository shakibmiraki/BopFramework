import { encryptService } from "./encrypt";
import { config } from "./../config";
import { api, createDynamicRequest } from "./api";
import { utilService } from "./utils";

const handleMenuResponse = (result) => {
  const aesKey = utilService.stringtoUtf8Byte(
    encryptService.rsaDecryptWithClientPrivateKey(result.data.meta.encryptKey)
  );

  encryptService.decryptResponse(result.data.data, aesKey, ["description", "title", "url", "versionName"]);

  result.data.data.categories.forEach((category) => {
    encryptService.decryptResponse(category, aesKey, ["id", "imageUrl", "type"]);

    category.icons.forEach((icon) => {
      encryptService.decryptResponse(icon, aesKey, [
        "categoryId",
        "description",
        "download",
        "id",
        "imageURL",
        "isStatic",
        "scheme",
        "packageName",
      ]);

      icon.names.forEach((name) => {
        encryptService.decryptResponse(name, aesKey, ["key", "value"]);
      });
      icon.titles.forEach((title) => {
        encryptService.decryptResponse(title, aesKey, ["key", "value"]);
      });
      icon.urLs.forEach((url, index) => {
        encryptService.decryptResponse(icon.urLs, aesKey, [index]);
        icon.urLs[index] = utilService.appendDomain(icon.urLs[index]);
      });
    });

    category.names.forEach((name) => {
      encryptService.decryptResponse(name, aesKey, ["key", "value"]);
    });

    category.titles.forEach((title) => {
      encryptService.decryptResponse(title, aesKey, ["key", "value"]);
    });
  });
};

const fetchMenu = async () => {
  const value = {
    versionName: encryptService.aesDynamicEncrypt(config.menu_version),
  };
  const request = await createDynamicRequest(value, utilService.getRandomDigit());
  return api
    .post(`${config.apiUrl}/GATEWAY/CONFIG/Icon/V1/GetIconsAsync`, request)
    .then((result) => {
      handleMenuResponse(result);
      return result;
    })
    .catch((error) => {
      utilService.handleError(error);
      throw error;
    });
};

export const appService = {
  fetchMenu,
};
