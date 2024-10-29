using System.ComponentModel;

namespace iCampus.MobileApp.Forms.UserModules.BooksReservation;

public class BindableStudentBookMasterView : INotifyPropertyChanged
	{
		public BindableStudentBookMasterView()
		{
		}
			public int StudentId
		{
			get;
			set;
		}

		public int StudentBookMasterId
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string ISBN
		{
			get;
			set;
		}

		public short Quantity
		{
			get;
			set;
		}

		public bool IsQuantityEditable
		{
			get;
			set;
		}

		public int GradeId
		{
			get;
			set;
		}

		public decimal Price
		{
			get;
			set;
		}

		public bool IsMandatory
		{
			get;
			set;
		}

		public string GradeName
		{
			get;
			set;
		}

		public bool IsSkipped
		{
			get;
			set;
		}

		bool _isChecked;
		public bool IsChecked
		{
			get
            {
				return _isChecked;
            }
			set
            {
				_isChecked = value;
				OnPropertyChanged("IsChecked");
			}
		}
		decimal _amount;
		public decimal Amount
		{
			get { return _amount; }
			set
			{
				_amount = value;
				OnPropertyChanged("Amount");
			}
		}
		string _currency;
		public string Currency
		{
			get { return _currency; }
			set
			{
				_currency = value;
				OnPropertyChanged("Currency");
			}
		}
		Color _selectedBookBackgroundColor;
		public Color SelectedBookBackgroundColor
		{
			get {
				_selectedBookBackgroundColor = IsMandatory ? Color.FromHex("#F2DEDE") : Colors.White;
				return _selectedBookBackgroundColor;
			}
			set
			{
				_selectedBookBackgroundColor = value;
				OnPropertyChanged("SelectedBookBackgroundColor");
			}
		}
		Color _quantityBorderColor;
		public Color QuantityBorderColor
		{
			get
			{
				_quantityBorderColor = IsQuantityEditable ? Color.FromHex("#f8ac54") : Colors.LightGray;
				return _quantityBorderColor;
			}
			set
			{
				_quantityBorderColor = value;
				OnPropertyChanged("QuantityBorderColor");
			}
		}
		bool _hasShadow;
		public bool HasShadow
		{
			get
			{
				_hasShadow = IsQuantityEditable ? true : false;
				return _hasShadow;
			}
			set
			{
				_hasShadow = value;
				OnPropertyChanged("HasShadow");
			}
		}
		bool _skippedStudentBooksSelectionAfterBooking;
		public bool SkippedStudentBooksSelectionAfterBooking
		{
			get
			{
				return _skippedStudentBooksSelectionAfterBooking;
			}
			set
			{
				_skippedStudentBooksSelectionAfterBooking = value;
				OnPropertyChanged("SkippedStudentBooksSelectionAfterBooking");
			}
		}
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged == null)
				return;

			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}