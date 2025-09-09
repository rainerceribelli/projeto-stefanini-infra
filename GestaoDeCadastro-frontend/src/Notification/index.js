import { notification } from "antd";

function showError(errorMessage, title, duration) {
  notification.error({
    message: title || "Erro",
    description: errorMessage,
    duration: duration === null || duration === undefined ? 30 : duration,
  });
}

function showInfo(infoMessage, title, duration) {
  notification.info({
    message: title || "Informação",
    description: infoMessage,
    duration: duration === null || duration === undefined ? 15 : duration,
  });
}

export { showError, showInfo };
