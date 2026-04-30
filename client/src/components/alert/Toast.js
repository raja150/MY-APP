import { Slide, toast } from 'react-toastify';

export const notifyError = (msg) => toast(msg, {
  transition: Slide,
  closeButton: true,
  autoClose: 2500,
  position: 'bottom-center',
  type: 'error'
});

export const notifySaved = (msg) => toast(msg || "Data saved successfully!", {
  transition: Slide,
  closeButton: true,
  autoClose: 2500,
  position: 'bottom-center',
  type: 'success'
});