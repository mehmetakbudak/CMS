export class Urls {
  static readonly BlogCategory = 'BlogCategory';
  static readonly NoteCategory = 'NoteCategory';
  static readonly Note = 'Note';
  static readonly User = 'User';
  static readonly Lookup = 'Lookup';
  static readonly Menu = 'Menu';
  static readonly Blog = 'Blog';
  static readonly Page = 'Page';
  static readonly Task = 'Task';
}

export function GetActiveStatus(): any[] {
  return [
    { value: true, name: 'Active' },
    { value: false, name: 'Passive' },
  ];
}
