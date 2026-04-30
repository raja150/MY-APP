import React, { Component, Fragment } from 'react';
import MetisMenu from 'react-metismenu';
import { connect } from 'react-redux';
import { withRouter } from 'react-router-dom';
import { setMenuData } from '../../reducers/MenuData';
import {
    setEnableMobileMenu
} from '../../reducers/ThemeOptions';
import APIService from '../../services/apiservice';
import Loading from 'components/Loader';
import SessionStorageService from 'services/SessionStorage';
 
class Nav extends Component {

    constructor(props) {
        super(props);
        this.state = { activeLinkTo: '/#', menu: [] } //JSON.parse(localStorage.getItem('pages')) ||
    }
    //state = {};

    toggleMobileSidebar = () => {
        let { enableMobileMenu, setEnableMobileMenu } = this.props;
        setEnableMobileMenu(!enableMobileMenu);
    }

    componentDidMount() { 
        // const { token } = JSON.parse(SessionStorageService.getUser() ? SessionStorageService.getUser() :'');
        this.setState({
            activeLinkTo: window.location.hash
        });
        if (this.state.menu.length === 0) { 
           
            const info =  SessionStorageService.getUserInfo();
            if (info) {
                const infoJson = JSON.parse(info);
                // APIService.getAsync(`Role/GetMenu/${infoJson.roleId}`).then((reponse) => {
                //     if (Array.isArray(reponse.data)) {
                //         const menu = [{ icon: 'ForemostIcons_Outline-01ramak-' , label: 'Dashboard', to: '#/Dashboard/User' }];
                //         reponse.data.forEach((item, k) => {
                //             let groupIdx = menu.findIndex(g => g.id === item.page.group.id);
                //             if (groupIdx < 0) {
                //                 menu.push({ id: item.page.group.id, icon: item.page.group.icon, label: item.page.group.name, content: [] });
                //                 groupIdx = menu.length - 1;
                //             }
                //             menu[groupIdx].content.push({ icon: item.page.icon, label: item.page.displayName, to: item.page.path });
                //         });
                //         this.setState({ menu: menu });
                       
                //         // localStorage.setItem('pages', JSON.stringify(menu));
                //     }
                // })
               
                this.props.menu.data ? this.renderOnce(this.props.menu) : 
                APIService.getAsync(`Role/GetMenu/${infoJson.roleId}`).then((response) => {
                    
                   this.renderOnce(response)
                })
            }
    
        }
    }

    renderOnce(response) {
        if (Array.isArray(response.data)) { 
             
            const menu = [{ icon: 'HRIS_Payroll-Templates', label: 'Dashboard', to: '#/Dashboard/User' }];
            response.data.forEach((item, k) => {
                let groupIdx = menu.findIndex(g => g.id === item.groupId);
                if (groupIdx < 0) {
                    menu.push({ id: item.groupId, icon: item.groupIcon, label: item.groupName, content: [] });
                    groupIdx = menu.length - 1;
                }
                menu[groupIdx].content.push({ icon: item.pageIcon, label: item.pageName, to: item.pagePath });
            });
            this.setState({ menu: menu });
        } 
    }

    componentWillReceiveProps() {
        this.setState({
            activeLinkTo: window.location.hash
        });
    }

    render() {
        return (
            <Fragment>
                {/* <h5 className="app-sidebar__heading">Menu</h5> */}
                {
                    this.state.menu.length > 0 ?

                        <MetisMenu
                            //content={MainNav} 
                            content={this.state.menu}
                            activeLinkTo={this.state.activeLinkTo}
                            onSelected={this.toggleMobileSidebar}
                            // activeLinkFromLocation 
                            className="vertical-nav-menu"
                            iconNamePrefix=""
                            classNameStateIcon="pe-7s-angle-down" /> : <Loading />
                }
                {/* <h5 className="app-sidebar__heading">UI Components</h5>
                <MetisMenu content={ComponentsNav}  onSelected={this.toggleMobileSidebar} activeLinkFromLocation className="vertical-nav-menu" iconNamePrefix="" classNameStateIcon="pe-7s-angle-down"/>
                <h5 className="app-sidebar__heading">Dashboard Widgets</h5>
                <MetisMenu content={WidgetsNav}  onSelected={this.toggleMobileSidebar} activeLinkFromLocation className="vertical-nav-menu" iconNamePrefix="" classNameStateIcon="pe-7s-angle-down"/>
                <h5 className="app-sidebar__heading">Forms</h5>
                <MetisMenu content={FormsNav}  onSelected={this.toggleMobileSidebar} activeLinkFromLocation className="vertical-nav-menu" iconNamePrefix="" classNameStateIcon="pe-7s-angle-down"/>
                <h5 className="app-sidebar__heading">Charts</h5>
                <MetisMenu content={ChartsNav}  onSelected={this.toggleMobileSidebar} activeLinkFromLocation className="vertical-nav-menu" iconNamePrefix="" classNameStateIcon="pe-7s-angle-down"/> */}
            </Fragment>
        );
    }

    isPathActive(path) {
        return this.props.location.pathname.startsWith(path);
    }
}
const mapStateToProps = state => ({
    enableMobileMenu: state.ThemeOptions.enableMobileMenu,
    menu : state.MenuData.menuData,
});

const mapDispatchToProps = dispatch => ({

    setEnableMobileMenu: enable => dispatch(setEnableMobileMenu(enable)),
    setMenuData: data => {
        dispatch(setMenuData(data));
    }

});
export default connect(mapStateToProps, mapDispatchToProps)(withRouter(Nav));