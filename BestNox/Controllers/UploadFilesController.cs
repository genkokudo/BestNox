using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BestNox.Data;
using BestNox.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace BestNox.Controllers
{
    [Authorize]
    public class UploadFilesController : Controller
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// パス取得に使用する
        /// </summary>
        private IHostingEnvironment _hostingEnvironment = null;

        public UploadFilesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: UploadFiles
        public async Task<IActionResult> Index()
        {
            // 自分のファイルを検索
            // 作成順に並べ替える
            return View(await _context.UploadFiles.Where(f => f.UpdatedBy == User.Identity.Name).OrderByDescending(f => f.CreatedDate).ToListAsync());
        }

        // GET: UploadFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadFile = await _context.UploadFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uploadFile == null)
            {
                return NotFound();
            }

            // 公開用アドレスの表示
            if (uploadFile.IsPublic)
            {
                ViewData["Url"] = Path.Combine($"{Request.Scheme}://{Request.Host}{Request.Path}", SystemConstants.PublicUploads, uploadFile.TmpFilename);
            }
            return View(uploadFile);
        }

        /// <summary>
        /// ファイルのダウンロード
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadFile = await _context.UploadFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uploadFile == null)
            {
                return NotFound();
            }
            // 保存先
            string targetFile = getTargetPath(uploadFile);

            return File(new FileStream(targetFile, FileMode.Open), uploadFile.ContentType, uploadFile.Filename);
        }

        // GET: UploadFiles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UploadFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(List<IFormFile> files, [Bind("Id,Comment,Filename,TmpFilename,IsPublic,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted")] UploadFile uploadFile)
        {
            if (ModelState.IsValid)
            {
                // ファイル確認
                if (files.Count == 0)
                {
                    ViewData["Error"] = SystemConstants.NoFileError;
                    return View(uploadFile);
                }

                // アップロード処理
                foreach (var formFile in files)
                {
                    // 一時ファイルのパスを取得
                    var filePath = Path.GetTempFileName();
                    // 各ファイルについて、ストリームを作成して
                    // その一時ファイルパスにコピー
                    if (formFile.Length > 0)
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                    }
                    // 保存時は拡張子だけ変えてサーバに置いて、元ファイル名だけ記憶していたら十分だと思います。

                    // サイズ
                    float size = formFile.Length;
                    size = (float)Math.Round(size / 1024f / 1024f, 2);
                    uploadFile.Size = size;
                    // コメント
                    if (string.IsNullOrEmpty(uploadFile.Comment))
                    {
                        uploadFile.Comment = SystemConstants.NoComment;
                    }
                    // 拡張子
                    var extension = Path.GetExtension(formFile.FileName);   // ".png"とかを取得
                    // 元ファイル名
                    uploadFile.Filename = formFile.FileName;
                    // 一時ファイル名
                    uploadFile.TmpFilename = Path.GetFileNameWithoutExtension(filePath) + extension;
                    // 保存先
                    string targetFile = getTargetPath(uploadFile);

                    // ディレクトリが無ければ作成
                    SafeCreateDirectory(Path.GetDirectoryName(targetFile));    // パスが無ければ作成
                    // 移動
                    System.IO.File.Move(filePath, targetFile);
                    
                    // 登録データ作成
                    var data = new UploadFile();
                    data.Comment = uploadFile.Comment;
                    data.Filename = uploadFile.Filename;
                    data.IsPublic = uploadFile.IsPublic;
                    data.Size = uploadFile.Size;
                    data.TmpFilename = uploadFile.TmpFilename;
                    data.ContentType = formFile.ContentType;

                    // 登録
                    _context.Add(data);
                }
                await _context.SaveChangesAsync(User.Identity.Name);
                return RedirectToAction(nameof(Index));
            }
            return View(uploadFile);
        }

        /// <summary>
        /// 実際のファイルの置き場所を取得する
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>
        private string getTargetPath(UploadFile uploadFile)
        {
            return getTargetPath(uploadFile.IsPublic, uploadFile.TmpFilename);
        }

        /// <summary>
        /// 実際のファイルの置き場所を取得する
        /// </summary>
        /// <returns></returns>
        private string getTargetPath(bool isPublic, string tmpFilename)
        {
            if (isPublic)
            {
                return Path.Combine(_hostingEnvironment.ContentRootPath, SystemConstants.PublicUploadsDirectry, tmpFilename);
            }
            return Path.Combine(_hostingEnvironment.ContentRootPath, SystemConstants.PrivateUploadsDirectry, tmpFilename);
        }

        // GET: UploadFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadFile = await _context.UploadFiles.FindAsync(id);
            if (uploadFile == null)
            {
                return NotFound();
            }
            uploadFile.IsPublicOld = uploadFile.IsPublic;
            return View(uploadFile);
        }

        // POST: UploadFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comment,Filename,TmpFilename,IsPublic,CreatedBy,CreatedDate,UpdatedBy,UpdatedDate,IsDeleted,IsPublicOld")] UploadFile uploadFile)
        {
            if (id != uploadFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // ファイルの移動
                    if (uploadFile.IsPublicOld != uploadFile.IsPublic)
                    {
                        System.IO.File.Move(getTargetPath(uploadFile.IsPublicOld, uploadFile.TmpFilename), getTargetPath(uploadFile));
                    }
                    _context.Update(uploadFile);     // 入力の状態で保存
                    await _context.SaveChangesAsync(User.Identity.Name);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UploadFileExists(uploadFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(uploadFile);
        }

        // GET: UploadFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var uploadFile = await _context.UploadFiles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uploadFile == null)
            {
                return NotFound();
            }

            return View(uploadFile);
        }

        // POST: UploadFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var uploadFile = await _context.UploadFiles.FindAsync(id);
            _context.UploadFiles.Remove(uploadFile);
            await _context.SaveChangesAsync(User.Identity.Name);
            System.IO.File.Delete(getTargetPath(uploadFile));// ファイルの削除
            return RedirectToAction(nameof(Index));
        }

        private bool UploadFileExists(int id)
        {
            return _context.UploadFiles.Any(e => e.Id == id);
        }

        /// <summary>
        /// 指定したパスにディレクトリが存在しない場合
        /// すべてのディレクトリとサブディレクトリを作成します
        /// </summary>
        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }
            return Directory.CreateDirectory(path);
        }
    }
}
