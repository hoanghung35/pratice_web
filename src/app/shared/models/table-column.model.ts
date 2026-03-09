import { TemplateRef } from '@angular/core';

export interface TableColumn<T> {
    key?: keyof T;                       // optional when you use a template
    label: string;
    render?: (row: T) => string;
    template?: TemplateRef<{ $implicit: T }>;
}
