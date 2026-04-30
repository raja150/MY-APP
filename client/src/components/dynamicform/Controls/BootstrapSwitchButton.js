import React from 'react';
import {FormGroup} from 'reactstrap';
import '../../../../src/assets/components/forms/toggle-switch/_switch-main.scss';

class BootstrapSwitchButton extends React.Component {
	constructor(props) {
		super(props);
         
		this.state = {
			checked:  this.props.value ? this.props.value : false,
			disabled: typeof this.props.disabled === 'boolean' ? this.props.disabled : false,
			onlabel: this.props.values[0].text || 'on',
			offlabel: this.props.values[1].text || 'off',
			onstyle: this.props.onstyle || 'success',
			offstyle: this.props.offstyle || 'danger',
			size: this.props.size || '',
			style: this.props.style || '',
			width: this.props.width || null,
			height: this.props.height || null,
		};
	}

	componentDidUpdate(_, prevState) {
		const { value } = this.props;
		
		if (typeof value === 'boolean' && value !== prevState.checked) {
			
			this.setState({ checked : value });
		}
	}

	toggle = event => {						
		this.state.checked ? this.off() : this.on();
		//this.props.handlevaluechange(this.props.name, this.state.checked, {})		
	};

	off = () => {
		if (!this.state.disabled) {
			this.setState({
				checked: false,
			});
			if (this.props.handlevaluechange) this.props.handlevaluechange(this.props.name, false, {});
		}
	};
	on = () => {
		if (!this.state.disabled) {
			this.setState({
				checked: true,
			});
			if (this.props.handlevaluechange) this.props.handlevaluechange(this.props.name, true, {});
		}
	};
	enable = () => {
		this.setState({
			disabled: false,
		});
	};
	disable = () => {
		this.setState({
			disabled: true,
		});
	};

	render = () => {
		let switchStyle = {};
		//this.state.width ? (switchStyle.width = this.state.width + 'px') : null;
		// this.state.height ? (switchStyle.height = this.state.height + 'px') : null;
		switchStyle.width = this.state.width ? (this.state.width + 'px') : null;
		switchStyle.height = this.state.height ? (this.state.height + 'px') : null;
		let labelStyle = {};
		if (this.state.height) labelStyle.lineHeight = 'calc(' + this.state.height + 'px * 0.8)';

		return (
			<FormGroup>
				 <legend className="text-capitalize font-weight-normal mb-0" htmlFor={this.props.name}>{this.props.label}</legend>
				<div
				className={
					'switch btn ' +
					(this.state.checked ? 'on btn-' + this.state.onstyle : 'off btn-' + this.state.offstyle) +
					(this.state.size ? ' btn-' + this.state.size : '') +
					(this.state.style ? ' ' + this.state.style : '')
				}
				style={switchStyle}
				onClick={this.toggle}
				id={this.props.name}
			>
				<div className="switch-group">
					<span className={'switch-on btn btn-' + this.state.onstyle + (this.state.size ? ' btn-' + this.state.size : '')} style={labelStyle}>
						{this.state.onlabel}
					</span>
					<span className={'switch-off btn btn-' + this.state.offstyle + (this.state.size ? ' btn-' + this.state.size : '')} style={labelStyle}>
						{this.state.offlabel}
					</span>
					<span className={'switch-handle btn btn-light' + (this.state.size ? 'btn-' + this.state.size : '')} />
				</div>
			</div>
			</FormGroup>
		);
	};
}

export default BootstrapSwitchButton