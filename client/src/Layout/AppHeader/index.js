import cx from 'classnames';
import React, { Fragment } from 'react';
import ReactCSSTransitionGroup from 'react-addons-css-transition-group';
import { connect } from 'react-redux';
import HeaderLogo from '../AppLogo';
// import SearchBox from './Components/SearchBox';
// import MegaMenu from './Components/MegaMenu';
import UserBox from './Components/UserBox';
// import HeaderRightDrawer from "./Components/HeaderRightDrawer";
import { Base64 } from 'js-base64';
// import HeaderDots from "./Components/HeaderDots";
import SessionStorageService from 'services/SessionStorage';

class Header extends React.Component {
    constructor(props) {
        super(props)

        this.state = {
            branches: [],
            selBranch: '',
            user: JSON.parse(SessionStorageService.getUser())
        }
    }
    componentDidMount() {

        // const info = localStorage.getItem('info');
        // if (info) {
        //     this.setState({
        //         infoJson: JSON.parse(info)
        //     })
        // }
        // this.fetchData();
    }

    // handlebranchChange = async (name, value, { selected }) => {      

    //     if (selected === undefined) {
    //         localStorage.removeItem('branch')
    //         this.setState({ ...this.state, selBranch: value })
    //     } else {
    //         localStorage.setItem('branch', JSON.stringify(selected));
    //         this.setState({ ...this.state, selBranch: value })
    //     }

    // }

    render() {
        let {
            headerBackgroundColor,
            enableMobileMenuSmall,
            enableHeaderShadow
        } = this.props;

        return (
            <Fragment>
                <ReactCSSTransitionGroup
                    component="div"
                    className={cx("app-header", headerBackgroundColor, { 'header-shadow': enableHeaderShadow })}
                    transitionName="HeaderAnimation"
                    transitionAppear={true}
                    transitionAppearTimeout={1500}
                    transitionEnter={false}
                    transitionLeave={false}>

                    <HeaderLogo />

                    <div className={cx(
                        "app-header__content",
                        { 'header-mobile-open': enableMobileMenuSmall },
                    )}>
                        <div className="app-header-left">
                            {/* <SearchBox/>
                            <MegaMenu/> */}
                        </div>
                        <div className="app-header-right">
                            {/* <HeaderDots/> */}
                            {/* <DropdownList
                                allowCreate={"onFilter"} filter={true}
                                data={this.state.branches} className='form-control p-0 form-control-sm'
                                valueField='id'
                                textField='branchName'
                                placeholder={'Select a branch'}
                                onChange={this.handlebranchChange}
                                id="searchClinic" name="searchClinic"
                                style={{ width: '10rem' }}
                                value={this.state.selBranch}
                            /> */}
                            {/* <div style={{ minWidth: '200px' }}>
                                <RWDropdownList {...{
                                    name: 'branchId', label: '', valueField: 'id', textField: 'branchName',
                                    value: this.state.selBranch, type: 'string', values: this.state.branches,
                                    error: '', touched: true
                                }} handlevaluechange={this.handlebranchChange} />
                            </div> */}
                            <form target='_blank' action="https://smartnet.avontix.com/login.php"
                                method="post" id="id_my_form">
                                <input type='hidden' name='token' id='token' value={this.state.user ? Base64.encode(this.state.user.token) : ''} />
                                <button type="submit" className="btn btn-primary">SmartNet</button>
                            </form>
                            <UserBox />
                            {/* <HeaderRightDrawer/> */}
                        </div>
                    </div>
                </ReactCSSTransitionGroup>
            </Fragment >
        );
    }
}

const mapStateToProps = state => ({
    enableHeaderShadow: state.ThemeOptions.enableHeaderShadow,
    closedSmallerSidebar: state.ThemeOptions.closedSmallerSidebar,
    headerBackgroundColor: state.ThemeOptions.headerBackgroundColor,
    enableMobileMenuSmall: state.ThemeOptions.enableMobileMenuSmall,
});

const mapDispatchToProps = dispatch => ({});

export default connect(mapStateToProps, mapDispatchToProps)(Header);