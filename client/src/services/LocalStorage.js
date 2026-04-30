class LocalStorageService {

    async setSearchData(schData, page, pageSize, screenUrl) {

        localStorage.setItem('SearchData', JSON.stringify({
            searchData: schData,
            page: page,
            size: pageSize,
            screenUrl: screenUrl
        }))
    }
    async getSearchData() {
        return await localStorage.getItem('SearchData')
    }
    async removeSearchData() {
        return await localStorage.removeItem('SearchData');
    }
    setPwdExpire(jwtToken) {
        localStorage.setItem('passwordExp', JSON.stringify({ token: jwtToken }));
    }
    removePwdExpire() {
        localStorage.removeItem('passwordExp');
    }
    async setUserToken(jwtToken) {
        localStorage.setItem('user', JSON.stringify({ token: jwtToken }));
    }
    async setRefreshToken(refreshToken) {
        localStorage.setItem('refresh_token', JSON.stringify({ refresh_token: refreshToken }))
    }
    async setUserInfo(name,no, userId, roleId) {
        localStorage.setItem('info', JSON.stringify({ name: name, userId: userId, roleId: roleId, no: no }));
    }
    removeUserInfo() {
        localStorage.removeItem('info')
    }
    removeUser() {
        localStorage.removeItem('user')
    }
    removeRefreshToken() {
        localStorage.removeItem('refresh_token');
    }
}
export default new LocalStorageService()