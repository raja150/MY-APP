// import APIService from '../apiservice';
// const apiUrl = 'Leave/Approval/Leave';

// class LeaveApplicationService {

//     paginate() {
//         return { searchAPI: `${apiUrl}` };
//     }
//     async getAsync(id) {
//         return await APIService.getAsync(`${apiUrl}/${id}`);
//     }
//     async updateAsync(data) {
//         return await APIService.putAsync(`${apiUrl}`, data);
//     }
//     async GetLeavesByMonth(month) {
//         return await APIService.getAsync(`${apiUrl}/AllLeaves?month=${month}`)
//     }
//     LMApprovalPaginate() {
//         return { searchAPI: `${apiUrl}/LMApprovalPaginate` };
//     }
// }
// export default new LeaveApplicationService();