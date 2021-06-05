class EditAdmParameter extends HFSSystemUtil {
	constructor()
	{
		super();
		
		this.hideQueryString();
		
		//this._page = $('#admParameterView');
	}
	
	btnCancelClick(event) {
		event.preventDefault();
		//this._page[0].click();
		window.location.href = '/AdmParameter';
	}
	
}

$(function() {
	const editAdmParameter = new EditAdmParameter();
	
	$('#btnCancel').click(editAdmParameter.btnCancelClick.bind(editAdmParameter));
	
});
