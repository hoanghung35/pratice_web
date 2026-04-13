export interface DialogField {
    name: string;
    label: string;
    type: string;
    value?: any;
}

export interface DialogData {
    title: string;
    contentComponent?: any;
    payload?: any;
    okText?: string;
    cancelText?: string;
    confirmText?: string;
    type?: string;
    fields?: DialogField[];
}
