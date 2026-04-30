import axios from 'axios'; 
import SessionStorageService from 'services/SessionStorage';
class APIService {

  constructor() {
    let service = axios.create({
      baseURL: process.env.REACT_APP_API_ENDPOINT, //(process.env.NODE_ENV !== 'production') ? 'http://localhost:9000/api/' : ''
      headers: { csrf: 'token', 'Access-Control-Allow-Origin': '*' },
      responseType: 'json',
    });
    service.interceptors.response.use(this.handleSuccess, this.handleError);
    this.service = service;
  }

  url() {
    return process.env.REACT_APP_API_ENDPOINT;
  }

  handleSuccess(response) {
    return response;
  }

  handleError = async (err) => {
    //Check error as response, Reject promise if usual errors
    if (err && err.response && err.response.status === 401) {
      const originalRequest = err.config;
      return axios.post(`${process.env.REACT_APP_API_ENDPOINT}/Auth/Refresh`,
        {
          'AccessToken': this.token(),
          'RefreshToken': this.refresh_token()
        })
        .then((res) => {
          if (res.status !== 400) {
            this.setSession({ token: res.data.accessToken, refresh_token: res.data.refreshToken });
            originalRequest.headers.Authorization = "Bearer " + res.data.accessToken;
            return new Promise((resolve, reject) => { resolve(axios(originalRequest)) });
          }
        }, (e) => {
          document.location = '#/login';
          return Promise.reject({ message: "Your login session expired. Please sign in again.!" });
        });
    }
    return Promise.reject(err);
  }

  redirectTo = (document, path) => {
    document.location = path
  }

  refresh_token() {
    if (SessionStorageService.getRefreshToken()) {
      const { refresh_token } = JSON.parse(SessionStorageService.getRefreshToken());
      return refresh_token;
    }
  }

  token() {
    if (SessionStorageService.getUser()) {
      const { token } = JSON.parse(SessionStorageService.getUser());
      return token;
    }
    return '';
  }

  setSession(res) {
    SessionStorageService.setUser(res.token);
    SessionStorageService.setRefreshToken(res.refresh_token);
  }

  async getAsync(path) {
   
    return await this.service.request({
      method: 'GET',
      url: this.url() + path,
      headers: { 'Authorization': 'Bearer ' + this.token() }
    })
  }

  async postAsync(path, payload) {
    return await this.service.request({
      method: 'POST',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
        // 'Content-Type': 'multipart/form-data'
      }
    })
  }

  async putAsync(path, payload) {
    return await this.service.request({
      method: 'PUT',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
        // 'Content-Type': 'multipart/form-data'
      }
    })
  }

  async deleteAsync(path, payload) {
    return await this.service.request({
      method: 'DELETE',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
        // 'Content-Type': 'multipart/form-data'
      }
    })
  }

  async getBlobAsync(path) {
   
    return await this.service.request({
      method: 'GET',
      responseType: 'blob',
      url: this.url() + path,
      headers: { 'Authorization': 'Bearer ' + this.token() }
    })
  }

  get(path, callback) {
  
    return this.service.request({
      method: 'GET',
      url: this.url() + path,
      headers: { 'Authorization': 'Bearer ' + this.token() }
    }).then(
      (response) => callback(response.status, response.data)
    );
  }

  patch(path, payload, callback) {
    return this.service.request({
      method: 'PATCH',
      url: path,
      data: payload
    }).then((response) => callback(response.status, response.data));
  }

  post(path, payload, callback, exception) {
    return this.service.request({
      method: 'POST',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
      }
    })
      .then((response) => callback(response.status, response.data))
      .catch((error) => exception(error));
  }
  async postFormDataAsync(path, payload, callback, exception) {
     
     return await this.service.request({
      method: 'POST',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
        'Content-Type': 'multipart/form-data'
      }
    }) };
    async putFormDataAsync(path, payload, callback, exception) {
     
      return await this.service.request({
       method: 'PUT',
       url: this.url() + path,
       data: payload,
       headers: {
         'Authorization': 'Bearer ' + this.token(),
         'Content-Type': 'multipart/form-data'
       }
     }) };
  putformdata(path, payload, callback, exception) {
    return this.service.request({
      method: 'PUT',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
        'Content-Type': 'multipart/form-data'
      }
    })
      .then((response) => callback(response.status, response.data))
      .then((response) => exception(response));
  }

  put(path, payload, callback, exception) {
    return this.service.request({
      method: 'PUT',
      url: this.url() + path,
      data: payload,
      headers: {
        'Authorization': 'Bearer ' + this.token(),
      }
    })
      .then((response) => callback(response.status, response.data))
      .then((response) => exception(response));
  }
}

export default new APIService();