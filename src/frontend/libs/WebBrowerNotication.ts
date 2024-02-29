import Push, { type PushNotificationParams } from "push.js";

export default function SendPushNotification(
  title: string,
  params?: PushNotificationParams
) {
  let id = Date.now();
  const idpush = "" + id;
  console.log("Showing Push Notification", idpush);
  showNotification(title, idpush, params);
  // if (!Push.Permission.has()) {
  //   showNotification(title, idpush, params);
  // }
  // // if (Notify.isSupported())
  // else {
  //   Push.Permission.request(onPermissionGranted, onPermissionDenied);
  // }

  function onPermissionGranted() {
    console.log("Permission has been granted by the user");
    showNotification(title, idpush, params);
  }

  function onPermissionDenied() {
    console.warn("Permission has been denied by the user");
  }
}

const showNotification = (
  title: string,
  id: string,
  params?: PushNotificationParams
) => {
  Push.create(title, {
    body: "How's it hangin'?",
    icon: "/icon.png",
    //timeout: 4000,
    tag: id,
    onClick: function () {
      window.focus();
      Push.close(id);
    },
  });
};
