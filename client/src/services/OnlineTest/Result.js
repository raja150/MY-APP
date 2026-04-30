import APIService from '../apiservice'
const apiUrl = 'OnlineTest/Result'

class ResultService {

    paginate() {
        return { searchAPI: `${apiUrl}/paginate` }
    }
    async GetSummary(id) {
        return await APIService.getAsync(`${apiUrl}/Summary/${id}`)
    }

    async AllowReTake(empId, paperId) {
        return await APIService.putAsync(`${apiUrl}/ReTake/${empId}/${paperId}`, {});
    }
}
export default new ResultService()