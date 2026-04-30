class SessionStorageService {

    async setSearchData(schData, page, pageSize, screenUrl) {

        sessionStorage.setItem('SearchData', JSON.stringify({
            searchData: schData,
            page: page,
            size: pageSize,
            screenUrl: screenUrl
        }))
    }
    setPwdExpire(jwtToken, uerName) {
        sessionStorage.setItem('passwordExp', JSON.stringify({ token: jwtToken }));
        sessionStorage.setItem('uName', JSON.stringify({ name: uerName }));

    }
    setUserLogin(jwtToken, refreshToken, name, no, image, roleId) {
        sessionStorage.removeItem('passwordExp');
        sessionStorage.removeItem('uName');
        sessionStorage.setItem('user', JSON.stringify({ token: jwtToken }));
        sessionStorage.setItem('refresh_token', JSON.stringify({ refresh_token: refreshToken }));
        sessionStorage.setItem('info', JSON.stringify({ name: name, image: image, roleId: roleId, no: no }));
    }
    setUser(token) {
        sessionStorage.setItem('user', JSON.stringify({ token: token }));
    }
    setRefreshToken(refreshToken) {
        sessionStorage.setItem('refresh_token', JSON.stringify({ refresh_token: refreshToken }));
    }
    removeExpireUserInfo() {
        sessionStorage.removeItem('passwordExp');
        sessionStorage.removeItem('uName');
    }

    getSearchData() {
        return sessionStorage.getItem('SearchData');
    }
    getUser() {
        return sessionStorage.getItem('user');
    }
    getUserInfo() {
        return sessionStorage.getItem('info');
    }
    getBranch() {
        return sessionStorage.getItem('branch');
    }
    getRefreshToken() {
        return sessionStorage.getItem('refresh_token');
    }
    getPasswordExp() {
        return JSON.parse(sessionStorage.getItem('passwordExp'));
    }
    getUserName() {
        return JSON.parse(sessionStorage.getItem('uName'));
    }

    removeEmpInfo() {
        return sessionStorage.removeItem('empInfo');
    }
    removeUserInfo() {
        return sessionStorage.removeItem('info');
    }
    removePwdExpire() {
        return sessionStorage.removeItem('passwordExp');
    }
    removeUser() {
        return sessionStorage.removeItem('user');
    }
    removeRefreshToken() {
        return sessionStorage.removeItem('refresh_token');
    }
    removeSearchData() {
        return sessionStorage.removeItem('SearchData');
    }
    setPageSize(size = 10) {
        localStorage.setItem('pageSize', size)
    }
    getPageSize() {
        return JSON.parse(localStorage.getItem('pageSize'))
    }
    setResponse() {
        localStorage.setItem('response', 'true')
        localStorage.removeItem('response')
    }
}
export default new SessionStorageService()