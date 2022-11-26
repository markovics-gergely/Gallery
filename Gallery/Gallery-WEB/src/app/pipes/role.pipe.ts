import { Pipe, PipeTransform } from '@angular/core';
import { Roles } from 'models';

@Pipe({
  name: 'role',
})
export class RolePipe implements PipeTransform {
  private static lookup = ['Admin', 'Regular'];

  transform(value: Roles): string {
    return RolePipe.lookup[value];
  }
}
