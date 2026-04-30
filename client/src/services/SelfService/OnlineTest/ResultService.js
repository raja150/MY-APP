import APIService from '../../apiservice'
const apiUrl = 'SelfService/Result'

class ResultService {

    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }

    async GetPapers() {
        return await APIService.getAsync(`${apiUrl}/Paper`);
    }
}
export default new ResultService()