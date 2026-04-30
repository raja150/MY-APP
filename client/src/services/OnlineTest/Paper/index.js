import APIService from '../../apiservice'
const apiUrl = 'OnlineTest/Paper'

class PaperService {

    async GetById(id) {
        return await APIService.getAsync(`${apiUrl}/${id}`)
    }

    paginate() {
        return { searchAPI: `${apiUrl}/Paginate` }
    }
    
    async GetPapers() {
        return await APIService.getAsync(`${apiUrl}/papers`);
    }

    async duplicate(paperId){
        return await APIService.getAsync(`${apiUrl}/Duplicate/${paperId}`)
    }

    async UpdateAsync(data) {
        if (data.id) {
            return await APIService.putAsync(`${apiUrl}`, data)
        }
        return await APIService.postAsync(`${apiUrl}`, data)
    }
}
export default new PaperService()