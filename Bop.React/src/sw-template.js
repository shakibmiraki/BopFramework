/* eslint-disable no-undef */
if ("function" === typeof importScripts) {
  importScripts("https://storage.googleapis.com/workbox-cdn/releases/5.1.2/workbox-sw.js");

  /* global workbox */
  if (workbox) {
    // workbox.setConfig({ modulePathPrefix: "/workbox-v5.1.3/" });

    console.log("Workbox is loaded");

    /* injection point for manifest files.  */
    workbox.precaching.precacheAndRoute(self.__WB_MANIFEST);

    //image caching
    workbox.routing.registerRoute(
      /\.(?:png|gif|jpg|jpeg|svg)$/,
      new workbox.strategies.CacheFirst({
        cacheName: "images",
        plugins: [
          new workbox.expiration.ExpirationPlugin({
            maxEntries: 60,
            maxAgeSeconds: 5 * 24 * 60 * 60, // 5 Days
          }),
        ],
      })
    );

    // Font caching
    workbox.routing.registerRoute(
      /\.(?:woff2|woff|ttf)$/,
      new workbox.strategies.CacheFirst({
        cacheName: "fonts",
        plugins: [
          new workbox.expiration.ExpirationPlugin({
            maxEntries: 30,
            maxAgeSeconds: 5 * 24 * 60 * 60, // 5 Days
          }),
        ],
      })
    );

    // Cache Api
    // workbox.routing.registerRoute(
    //   ({ url }) => url.pathname.endsWith("/V1/GetSomething"),
    //   new workbox.strategies.CacheFirst({
    //     cacheName: "api-cache-1",
    //   })
    // );

    // JS, CSS caching
    workbox.routing.registerRoute(
      /\.(?:js|css|ico)$/,
      new workbox.strategies.StaleWhileRevalidate({
        cacheName: "static-resources",
        plugins: [
          new workbox.expiration.ExpirationPlugin({
            maxEntries: 60,
            maxAgeSeconds: 1 * 24 * 60 * 60, // 1 Days
          }),
        ],
      })
    );
  } else {
    console.log("Workbox could not be loaded. No Offline support");
  }
}
