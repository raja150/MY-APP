import APIService from '../apiservice'
const apiUrl = 'HelpDesk/HelpTopic'

class HelpTopicService {
    async HelpTopicsByDept(deptId) {
        return await APIService.getAsync(`${apiUrl}/Topics/${deptId}`)
    }
}
export default new HelpTopicService();